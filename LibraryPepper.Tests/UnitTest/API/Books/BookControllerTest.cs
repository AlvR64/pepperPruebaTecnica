using FluentAssertions;
using ISF.FAF_RF.Domain.Common;
using LibraryPepper.API.Books;
using LibraryPepper.API.Books.Models;
using LibraryPepper.Application.Books.Commands.DeleteBook;
using LibraryPepper.Application.Books.Commands.PublishBook;
using LibraryPepper.Application.Books.Queries;
using LibraryPepper.Application.Books.Queries.GetAll;
using LibraryPepper.Application.Books.Queries.GetBook;
using LibraryPepper.Domain.Dtos.Books;
using LibraryPepper.Domain.Entities;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LibraryPepper.Tests.UnitTest.API.Books
{
    public class BookControllerTest
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ISender> _sender;
        private readonly BookController _controller;

        private readonly CancellationToken _cancellationToken;

        public BookControllerTest()
        {
            _mapper = new Mock<IMapper>();
            _sender = new Mock<ISender>();
            _controller = new BookController(_sender.Object, _mapper.Object);

            _cancellationToken = new CancellationToken();
        }

        #region PublishBook
        [Fact]
        public async void ShouldPublishBook()
        {
            //Arrange
            var request = new BookPublishRQ { AuthorName = "Pepper Money", PublicationDate = DateTime.UtcNow, Title = "Prueba tecnica Pepper"};
            var command = new PublishBookCommand ( AuthorName: request.AuthorName, PublicationDate: request.PublicationDate, Title: request.Title );
            var bookDto = new BookDto { Id = 123, AuthorName = request.AuthorName, PublicationDate = request.PublicationDate, Title = request.Title };
            _mapper
                .Setup(m => m.Map<PublishBookCommand>(request))
                .Returns(command);
            _sender
                .Setup(s => s.Send(command, _cancellationToken))
                .ReturnsAsync(bookDto);

            //Act
            var result = await _controller.Publish(request, _cancellationToken);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult) result;
            okResult.Value.Should().BeOfType<BookDto>();
            var bookResult = (BookDto)okResult.Value!;
            bookResult.Should().BeEquivalentTo(bookDto);
            _mapper
                .Verify(m => m.Map<PublishBookCommand>(request), Times.Once);
            _sender
                .Verify(s => s.Send(command, _cancellationToken), Times.Once);
        }

        [Fact]
        public async void ShouldThrowErrorWhenSomethingUnhandledHappens()
        {
            //Arrange
            var request = new BookPublishRQ { AuthorName = "Pepper Money", PublicationDate = DateTime.UtcNow, Title = "Prueba tecnica Pepper" };
            var command = new PublishBookCommand(AuthorName: request.AuthorName, PublicationDate: request.PublicationDate, Title: request.Title);
            var appError = new AppException(AppError.UnhandleError);
            _mapper
                .Setup(m => m.Map<PublishBookCommand>(request))
                .Returns(command);
            _sender
                .Setup(s => s.Send(command, _cancellationToken))
                .ReturnsAsync(() => throw appError);

            //Act
            var result = async () => await _controller.Publish(request, _cancellationToken);

            //Assert
            await result.Should().ThrowAsync<AppException>().Where(e => e == appError);
            _mapper
                .Verify(m => m.Map<PublishBookCommand>(request), Times.Once);
            _sender
                .Verify(s => s.Send(command, _cancellationToken), Times.Once);
        }
        #endregion

        #region DeleteBook
        [Fact]
        public async void ShouldDeleteBook()
        {
            var request = 1;
            var command = new DeleteBookCommand(Id: request);
            _sender
                .Setup(s => s.Send(command, _cancellationToken))
                .ReturnsAsync(true);

            var result = await _controller.Delete(request, _cancellationToken);

            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.Value.Should().Be(true);
            _sender
                .Verify(s => s.Send(command, _cancellationToken), Times.Once);
        }

        [Fact]
        public async void ShouldThrowErrorWhenRequestIdForDeleteBookIsNotFound()
        {
            var request = 1;
            var command = new DeleteBookCommand(Id: request);
            var appError = new AppException(AppError.IdError);
            _sender
                .Setup(s => s.Send(command, _cancellationToken))
                .Returns(() => throw appError);

            var result = async () => await _controller.Delete(request, _cancellationToken);

            await result.Should().ThrowAsync<AppException>().Where(e => e == appError);
            _sender
                .Verify(s => s.Send(command, _cancellationToken), Times.Once);
        }
        #endregion

        #region GetById
        [Fact]
        public async void ShouldReturnBookById()
        {
            var request = 1;
            var query = new GetBookQuery(Id: request);
            var bookDto = new BookDto { Id = request, AuthorName = "Pepper Money", PublicationDate = DateTime.UtcNow, Title = "Prueba tecnica Pepper" };
            _sender
                .Setup(s => s.Send(query, _cancellationToken))
                .ReturnsAsync(bookDto);

            var result = await _controller.GetById(request, _cancellationToken);

            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.Value.Should().BeOfType<BookDto>();
            var bookResult = (BookDto)okResult.Value!;
            bookResult.Should().BeEquivalentTo(bookDto);
            _sender
                .Verify(s => s.Send(query, _cancellationToken), Times.Once);
        }

        [Fact]
        public async void ShouldThrowErrorWhenIdForGetBookIsNotFound()
        {
            var request = 1;
            var query = new GetBookQuery(Id: request);
            var appError = new AppException(AppError.IdError);
            _sender
                .Setup(s => s.Send(query, _cancellationToken))
                .Returns(() => throw appError);

            var result = async () => await _controller.GetById(request, _cancellationToken);

            await result.Should().ThrowAsync<AppException>().Where(e => e == appError);
            _sender
                .Verify(s => s.Send(query, _cancellationToken), Times.Once);
        }
        #endregion

        [Fact]
        public async void ShouldReturnAllBooks()
        {
            var query = new GetAllBooksQuery();
            var bookListDto = new List<BookDto> {
                new BookDto { Id = 1, Title = "Pepper", PublicationDate = new DateTime(2001, 1, 1), AuthorName = "Author 1" },
                new BookDto { Id = 2, Title = "Quijote", PublicationDate = new DateTime(2002, 2, 2), AuthorName = "Author 2" },
                new BookDto { Id = 3, Title = "Lord of the Rings", PublicationDate = new DateTime(2003, 3, 3), AuthorName = "Author 3" }
            };
            _sender
                .Setup(s => s.Send(query, _cancellationToken))
                .ReturnsAsync(bookListDto);

            var result = await _controller.GetAll(_cancellationToken);

            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.Value.Should().BeOfType<List<BookDto>>();
            var listBooksResult = (List<BookDto>)okResult.Value!;
            listBooksResult.Should().BeEquivalentTo(bookListDto);
            _sender
                .Verify(s => s.Send(query, _cancellationToken), Times.Once);
        }
    }
}
