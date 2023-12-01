using FluentAssertions;
using LibraryPepper.Application.Books.Queries.GetBook;
using LibraryPepper.Domain.Dtos.Books;
using LibraryPepper.Domain.Entities;
using LibraryPepper.Domain.Repositories;
using MapsterMapper;
using Moq;

namespace LibraryPepper.Tests.UnitTest.Application.Books.Queries.GetBook
{
    public class GetBookQueryTest
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IBookRepository> _bookRepository;
        private readonly GetBookQueryHandler _queryHandler;
        private readonly CancellationToken _cancellationToken;

        public GetBookQueryTest()
        {
            _mapper = new Mock<IMapper>();
            _bookRepository = new Mock<IBookRepository>();
            _queryHandler = new GetBookQueryHandler(_bookRepository.Object, _mapper.Object);
            _cancellationToken = new CancellationToken();
        }

        [Fact]
        public async void ShouldReturnBookById()
        {
            var requestId = 1;
            var query = new GetBookQuery(Id: requestId);
            var book = new Book { Id = 1, Title = "Pepper", PublicationDate = new DateTime(2001, 1, 1), AuthorName = "Author 1" };
            var bookDto = new BookDto { Id = 1, Title = "Pepper", PublicationDate = new DateTime(2001, 1, 1), AuthorName = "Author 1" };
            _bookRepository
                .Setup(x => x.GetById(query.Id, _cancellationToken))
                .ReturnsAsync(book);
            _mapper
                .Setup(x => x.Map<BookDto>(book))
                .Returns(bookDto);

            var result = await _queryHandler.Handle(query, _cancellationToken);

            result.Should().BeOfType<BookDto>();
            result.Should().BeEquivalentTo(bookDto);
            _bookRepository
                .Verify(x => x.GetById(query.Id, _cancellationToken), Times.Once);
        }
    }
}
