using LibraryPepper.Domain.Dtos.Books;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryPepper.Application.Books.Queries.GetAll
{
    public record GetAllBooksQuery() : IRequest<List<BookDto>>;
}
