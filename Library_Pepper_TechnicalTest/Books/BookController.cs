using LibraryPepper.API.Books.Models;
using LibraryPepper.Application.Books.Commands.DeleteBook;
using LibraryPepper.Application.Books.Commands.PublishBook;
using LibraryPepper.Application.Books.Queries;
using LibraryPepper.Application.Books.Queries.GetAll;
using LibraryPepper.Application.Books.Queries.GetBook;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace LibraryPepper.API.Books
{
    [ApiController]
    [Route("api/book")]
    public class BookController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public BookController(ISender sender, IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }

        [HttpPost("publish")]
        public async Task<IActionResult> Publish(BookPublishRQ request, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<PublishBookCommand>(request);
            var response = await _sender.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(new GetBookQuery(id), cancellationToken);
            return Ok(response);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var response = await _sender.Send(new GetAllBooksQuery(), cancellationToken);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            return Ok(await _sender.Send(new DeleteBookCommand(id), cancellationToken));
        }
    }
}
