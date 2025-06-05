using LibraryAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services
{
    public class BorrowingService
    {
        private LibraryDbContext _context;

        public BorrowingService(LibraryDbContext context)
        {
            _context = context;
        }

        public string BorrowBook(BorrowingsEntity bookToBorrow, string oid)
        {
            string message = string.Empty;

            var book = _context.Books.FirstOrDefault(x => x.Id == bookToBorrow.BookId);
            var borrowingSystem = _context.Borrowings.FirstOrDefault(x => x.UserOid == bookToBorrow.UserOid && x.BookId == bookToBorrow.BookId);

            if (book == null)
            {
                return "Book not found";
            }

            if (book.IsAvailable && borrowingSystem == null)
            {
                book.IsAvailable = false;
                book.BorrowedCount++;

                bookToBorrow.BorrowedAt = DateTime.UtcNow;
                bookToBorrow.UserOid = oid;
                _context.Borrowings.Add(bookToBorrow);

                _context.SaveChanges();

                message = $"Title: {book.Title} | Author: {book.Author} | Release year: {book.YearPublished}";
            }
            else
            {
                message = "Book is currently unavailable.";
            }

            return message;
        }

        public string ReturnBook(BorrowingsEntity bookToReturn, string oid)
        {
            string message = string.Empty;

            var book = _context.Books.FirstOrDefault(x => x.Id == bookToReturn.BookId);
            var borrowingSystem = _context.Borrowings.FirstOrDefault(x => x.BookId == bookToReturn.BookId && x.UserOid == oid && x.ReturnedAt == null);

            if (book == null && borrowingSystem == null)
            {
                return "Book not found";
            }

            if (!book.IsAvailable && borrowingSystem != null)
            {
                book.IsAvailable = true;
                borrowingSystem.ReturnedAt = DateTime.UtcNow;

                _context.SaveChanges();

                message = $"Title: {book.Title} | Author: {book.Author} | Release year: {book.YearPublished}";
            }
            else
            {
                message = "Book is currently not borrowed!";
            }

            return message;
        }

        public List<BookEntity> GetBorrowedBooks(string? oid)
        {
            return _context.Borrowings
                .Where(x => x.UserOid == oid && x.ReturnedAt == null)
                .Include(x => x.Book)
                .Select(x => x.Book)
                .ToList();
        }
    }
}
