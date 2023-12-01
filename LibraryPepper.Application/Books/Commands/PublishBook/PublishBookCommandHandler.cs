using LibraryPepper.Domain.Dtos.Books;
using LibraryPepper.Domain.Entities;
using LibraryPepper.Domain.Repositories;
using MapsterMapper;
using MediatR;

namespace LibraryPepper.Application.Books.Commands.PublishBook
{
    public class PublishBookCommandHandler : IRequestHandler<PublishBookCommand, BookDto>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public PublishBookCommandHandler(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<BookDto> Handle(PublishBookCommand request, CancellationToken cancellationToken)
        {
            var mappedRequest = _mapper.Map<Book>(request);
            var bookEntity = await _bookRepository.Save(mappedRequest, cancellationToken);
            return _mapper.Map<BookDto>(bookEntity);
        }
    }
}
