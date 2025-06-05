using LibraryAPI.Data;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace LibraryAPI.Services
{
    public class LibraryService
    {
        private LibraryDbContext _context;


        public LibraryService(LibraryDbContext context)
        {
            _context = context;
        }

        public IEnumerable<BookEntity> ListBooks()
        {
            return _context.Books;
        }

        public BookEntity GetBookById(int id)
        {
            return _context.Books.FirstOrDefault(x => x.Id == id);
        }

        public BookEntity AddBook(BookEntity newBook)
        {
            var entity = new BookEntity()
            {
                Title = newBook.Title,
                Author = newBook.Author,
                ISBN = newBook.ISBN,
                YearPublished = newBook.YearPublished,
                IsAvailable = newBook.IsAvailable,
                BorrowedCount = newBook.BorrowedCount
            };

            _context.Books.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public BookEntity EditBook(BookEntity updatedBook, int id)
        {
            var entity = _context.Books.FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                return null;
            }

            entity.Title = updatedBook.Title;
            entity.Author = updatedBook.Author;
            entity.ISBN = updatedBook.ISBN;
            entity.YearPublished = updatedBook.YearPublished;
            entity.IsAvailable = updatedBook.IsAvailable;
            entity.BorrowedCount = updatedBook.BorrowedCount;

            _context.SaveChanges();
            return entity;
        }

        public bool DeleteBook(int id)
        {
            var entity = _context.Books.FirstOrDefault(x => x.Id == id);

            if (entity != null)
            {
                _context.Books.Remove(entity);
                _context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
