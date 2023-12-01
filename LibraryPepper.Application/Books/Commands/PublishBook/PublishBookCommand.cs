using LibraryPepper.Domain.Dtos.Books;
using MediatR;

namespace LibraryPepper.Application.Books.Commands.PublishBook
{
    public record PublishBookCommand(string Title, DateTime PublicationDate, string AuthorName) : IRequest<BookDto>;
}
