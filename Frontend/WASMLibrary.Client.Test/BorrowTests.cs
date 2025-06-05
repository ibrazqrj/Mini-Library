using LibraryAPI.Data;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WASMLibrary.Client.Test
{
    public class BorrowTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public BorrowTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task BorrowBook_BorrowIfBookIsAvailableTest()
        {
            using var scope = _factory.Services.CreateScope();
            var borrowingService = scope.ServiceProvider.GetRequiredService<BorrowingService>();
            var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

            var book = new BookEntity
            {
                Title = "TestbookAvailable",
                Author = "Tester",
                ISBN = "123-4567890123",
                YearPublished = 2020,
                IsAvailable = true,
                BorrowedCount = 1
            };
            context.Books.Add(book);

            var borrowed = new BorrowingsEntity
            {
                BookId = book.Id,
                UserOid = "abc123",
                BorrowedAt = DateTime.UtcNow
            };

            context.Borrowings.Add(borrowed);
            context.SaveChanges();

            var borrowedTwo = new BorrowingsEntity
            {
                BookId = book.Id,
                UserOid = "xyz456"
            };

            var result = borrowingService.BorrowBook(borrowedTwo, "xyz456");

            Assert.Equal($"Title: {book.Title} | Author: {book.Author} | Release year: {book.YearPublished}", result);
        }

        [Fact]
        public async Task ReturnBook_ReturnBookIfBorrowedTest()
        {
            using var scope = _factory.Services.CreateScope();
            var borrowingService = scope.ServiceProvider.GetRequiredService<BorrowingService>();
            var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

            var book = new BookEntity
            {
                Title = "TestBookBorrowed",
                Author = "Tester",
                ISBN = "123-4567890123",
                YearPublished = 2020,
                IsAvailable = false,
                BorrowedCount = 1
            };
            context.Books.Add(book);
            context.SaveChanges();

            var borrowed = new BorrowingsEntity
            {
                BookId = book.Id,
                UserOid = "abc123",
                BorrowedAt = DateTime.UtcNow
            };

            context.Borrowings.Add(borrowed);
            context.SaveChanges();

            var returned = new BorrowingsEntity
            {
                BookId = book.Id,
                UserOid = "abc123",
            };

            var result = borrowingService.ReturnBook(returned, "abc123");

            Assert.Equal($"Title: {book.Title} | Author: {book.Author} | Release year: {book.YearPublished}", result);
        }

        [Fact]
        public async Task BorrowBook_BorrowIfBookIsBorrowedTest()
        {
            using var scope = _factory.Services.CreateScope();
            var borrowingService = scope.ServiceProvider.GetRequiredService<BorrowingService>();
            var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

            var book = new BookEntity
            {
                Title = "Testbuch",
                Author = "Tester",
                ISBN = "123-4567890123",
                YearPublished = 2020,
                IsAvailable = false,
                BorrowedCount = 1
            };
            context.Books.Add(book);

            var borrowed = new BorrowingsEntity
            {
                BookId = book.Id,
                UserOid = "abc123",
                BorrowedAt = DateTime.UtcNow
            };

            context.Borrowings.Add(borrowed);
            context.SaveChanges();

            var borrowedTwo = new BorrowingsEntity
            {
                BookId = book.Id,
                UserOid = "xyz456"
            };

            var result = borrowingService.BorrowBook(borrowedTwo, "xyz456");

            Assert.Equal("Book is currently unavailable.", result);
        }

        [Fact]
        public async Task ReturnBook_ReturnBookIfNotBorrowedTest()
        {
            using var scope = _factory.Services.CreateScope();
            var borrowingService = scope.ServiceProvider.GetRequiredService<BorrowingService>();
            var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

            var book = new BookEntity
            {
                Title = "TestBookNotBorrowed",
                Author = "Tester",
                ISBN = "123-4567890123",
                YearPublished = 2020,
                IsAvailable = true,
                BorrowedCount = 1
            };
            context.Books.Add(book);
            context.SaveChanges();

            var returned = new BorrowingsEntity
            {
                BookId = book.Id,
                UserOid = "abc123",
            };

            var result = borrowingService.ReturnBook(returned, "abc123");

            Assert.Equal("Book is currently not borrowed!", result);
        }

        [Fact]
        public async Task BorrowBook_BookNotFoundMessage()
        {
            using var scope = _factory.Services.CreateScope();
            var borrowingService = scope.ServiceProvider.GetRequiredService<BorrowingService>();

            var book = new BorrowingsEntity
            {
                BookId = 214124,
                UserOid = "abc123"
            };

            var result = borrowingService.BorrowBook(book, "xyz456");

            Assert.Equal("Book not found", result);
        }


        [Fact]
        public async Task ReturnBook_BookNotFoundMessage()
        {
            using var scope = _factory.Services.CreateScope();
            var borrowingService = scope.ServiceProvider.GetRequiredService<BorrowingService>();

            var returned = new BorrowingsEntity
            {
                BookId = 123124124,
                UserOid = "abc123",
            };

            var result = borrowingService.ReturnBook(returned, "abc123");

            Assert.Equal("Book not found", result);
        }
    }
}
