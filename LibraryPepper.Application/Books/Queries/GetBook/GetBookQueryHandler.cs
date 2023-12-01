using LibraryPepper.Domain.Dtos.Books;
using LibraryPepper.Domain.Repositories;
using MapsterMapper;
using MediatR;

namespace LibraryPepper.Application.Books.Queries.GetBook
{
    public class GetBookQueryHandler : IRequestHandler<GetBookQuery, BookDto>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public GetBookQueryHandler(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<BookDto> Handle(GetBookQuery request, CancellationToken cancellationToken)
        {
            var bookFound = await _bookRepository.GetById(request.Id, cancellationToken);
            return _mapper.Map<BookDto>(bookFound);
        }
    }
}
