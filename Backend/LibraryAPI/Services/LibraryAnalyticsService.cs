using LibraryAPI.Data;
using LibraryAPI.Models;
using Microsoft.Data.SqlClient;

namespace LibraryAPI.Services
{
    public class LibraryAnalyticsService
    {
        private readonly LibraryDbContext _context;

        public LibraryAnalyticsService(LibraryService libraryService, LibraryDbContext context)
        {
            _context = context;
        }

        public string AnalyzeMostBorrowedBookTitle()
        {
            var borrowedCount = _context.Books
                .OrderByDescending(x => x.BorrowedCount)
                .Select(x => new { x.Title, x.BorrowedCount })
                .FirstOrDefault();

            if (borrowedCount == null)
            {
                return "There is no book entry.";
            }

            string message = $"{borrowedCount.Title}";

            return message;
        }

        public string AnalyzeMostBorrowedBookTimes()
        {
            var borrowedCount = _context.Books
                .OrderByDescending(x => x.BorrowedCount)
                .Select(x => new { x.Title, x.BorrowedCount })
                .FirstOrDefault();

            if (borrowedCount == null)
            {
                return "There is no book entry.";
            }

            string message = $"{borrowedCount.BorrowedCount}";

            return message;
        }

        public string AnalyzeTotalUserAmount()
        {
            var userExists = _context.Users.Select(x => x.Id).Count();

            string message = $"{userExists}";

            return message;
        }

        public string AnalyzeCurrentlyBorrowed()
        {
            int currentlyBorrowed = _context.Books.Where(x => x.IsAvailable == false).Count();

            string message = $"{currentlyBorrowed}";
            return message; 
        }

        public string AnalyzeTotalBorrowedBooks()
        {
            int totalBorrowed = _context.Books.Sum(x => x.BorrowedCount);

            string message = $"{totalBorrowed}";
            return message;
        }
    }
}
