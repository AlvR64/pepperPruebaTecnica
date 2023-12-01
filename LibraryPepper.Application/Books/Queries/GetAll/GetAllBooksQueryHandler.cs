using LibraryPepper.Domain.Dtos.Books;
using LibraryPepper.Domain.Repositories;
using MapsterMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryPepper.Application.Books.Queries.GetAll
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, List<BookDto>>
    {
        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepository;

        public GetAllBooksQueryHandler(IMapper mapper, IBookRepository bookRepository)
        {
            _mapper = mapper;
            _bookRepository = bookRepository;
        }

        public async Task<List<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            var booksFound = await _bookRepository.GetAll(cancellationToken);
            return _mapper.Map<List<BookDto>>(booksFound);
        }
    }
}
