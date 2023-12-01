using FluentAssertions;
using ISF.FAF_RF.Domain.Common;
using LibraryPepper.API.Books.Models;
using LibraryPepper.Application.Books.Commands.DeleteBook;
using LibraryPepper.Application.Books.Commands.PublishBook;
using LibraryPepper.Domain.Dtos.Books;
using LibraryPepper.Domain.Entities;
using LibraryPepper.Domain.Repositories;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryPepper.Tests.UnitTest.Application.Books.Commands.PublishBook
{
    public class PublishBookCommandTest
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IBookRepository> _bookRepository;
        private readonly PublishBookCommandHandler _commandHandler;
        private readonly CancellationToken _cancellationToken;

        public PublishBookCommandTest()
        {
            _mapper = new Mock<IMapper>();
            _bookRepository = new Mock<IBookRepository>();
            _commandHandler = new PublishBookCommandHandler(_bookRepository.Object, _mapper.Object);
            _cancellationToken = new CancellationToken();
        }

        [Fact]
        public async void ShouldPublishBook()
        {
            var command = new PublishBookCommand(Title: "Pepper", PublicationDate: new DateTime(2001, 1, 1), AuthorName: "Author 1" );
            var bookRequest = new Book { Title = "Pepper", PublicationDate = new DateTime(2001, 1, 1), AuthorName = "Author 1" };
            var bookEntity = new Book { Id = 1, AuthorName = bookRequest.AuthorName, PublicationDate = bookRequest.PublicationDate, Title = bookRequest.Title };
            var bookDto = new BookDto { Id = 1, AuthorName = bookRequest.AuthorName, PublicationDate = bookRequest.PublicationDate, Title = bookRequest.Title };
            _mapper
                .Setup(x => x.Map<Book>(command))
                .Returns(bookRequest);
            _mapper
                .Setup(x => x.Map<BookDto>(bookEntity))
                .Returns(bookDto);
            _bookRepository
                .Setup(x => x.Save(bookRequest, _cancellationToken))
                .ReturnsAsync(bookEntity);

            var result = await _commandHandler.Handle(command, _cancellationToken);

            result.Should().BeOfType<BookDto>();
            result.Should().BeEquivalentTo(bookDto);
            _bookRepository
                .Verify(x => x.Save(bookRequest, _cancellationToken), Times.Once);
        }
    }
}
