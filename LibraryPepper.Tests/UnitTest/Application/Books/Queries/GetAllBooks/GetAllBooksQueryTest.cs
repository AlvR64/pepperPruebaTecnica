using FluentAssertions;
using LibraryPepper.Application.Books.Queries.GetAll;
using LibraryPepper.Domain.Dtos.Books;
using LibraryPepper.Domain.Entities;
using LibraryPepper.Domain.Repositories;
using MapsterMapper;
using Moq;

namespace LibraryPepper.Tests.UnitTest.Application.Books.Queries.GetAllBooks
{
    public class GetBookQueryTest
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IBookRepository> _bookRepository;
        private readonly GetAllBooksQueryHandler _queryHandler;
        private readonly CancellationToken _cancellationToken;

        public GetBookQueryTest()
        {
            _mapper = new Mock<IMapper>();
            _bookRepository = new Mock<IBookRepository>();
            _queryHandler = new GetAllBooksQueryHandler(_mapper.Object, _bookRepository.Object);
            _cancellationToken = new CancellationToken();
        }

        [Fact]
        public async void ShouldReturnAllBooks()
        {
            var query = new GetAllBooksQuery();
            var books = new List<Book> { new Book { Id = 1, Title = "Pepper", PublicationDate = new DateTime(2001, 1, 1), AuthorName = "Author 1" } };
            var booksDto = new List<BookDto> { new BookDto { Id = 1, Title = "Pepper", PublicationDate = new DateTime(2001, 1, 1), AuthorName = "Author 1" } };
            _bookRepository
                .Setup(x => x.GetAll(_cancellationToken))
                .ReturnsAsync(books);
            _mapper
                .Setup(x => x.Map<List<BookDto>>(books))
                .Returns(booksDto);

            var result = await _queryHandler.Handle(query, _cancellationToken);

            result.Should().BeOfType<List<BookDto>>();
            result.Should().BeEquivalentTo(booksDto);
            _bookRepository
                .Verify(x => x.GetAll(_cancellationToken), Times.Once);
        }
    }
}
