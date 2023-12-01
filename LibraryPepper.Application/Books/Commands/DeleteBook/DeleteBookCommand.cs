using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryPepper.Application.Books.Commands.DeleteBook
{
    public record DeleteBookCommand(int Id) : IRequest<bool>;
}
