using LibraryPepper.Domain.Dtos.Books;
using MediatR;

namespace LibraryPepper.Application.Books.Queries.GetBook
{
    public record GetBookQuery(int Id) : IRequest<BookDto>;
}
