using LibraryPepper.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryPepper.Domain.Repositories
{
    public interface IBookRepository
    {
        public Task<Book> Save(Book book, CancellationToken cancellationToken);
        public Task<Book> GetById(int id, CancellationToken cancellationToken);
        public Task<List<Book>> GetAll(CancellationToken cancellationToken);
        public Task<bool> Delete(Book book, CancellationToken cancellationToken);
    }
}
