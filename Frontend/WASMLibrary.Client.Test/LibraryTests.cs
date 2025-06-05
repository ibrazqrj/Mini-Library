using LibraryAPI.Data;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WASMLibrary.Client.Test
{
    public class LibraryTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public LibraryTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        //Tests with valid values
        [Fact]
        public async Task AddBookTest()
        {
            using var scope = _factory.Services.CreateScope();
            var library = scope.ServiceProvider.GetRequiredService<LibraryService>();

            var bookToAdd = new BookEntity
            {
                Title = "Hinzugefügter Titel",
                Author = "Hinzugefügter Author",
                ISBN = "111111111",
                YearPublished = 2025,
                IsAvailable = true,
                BorrowedCount = 0
            };

            var result = library.AddBook(bookToAdd);

            Assert.NotNull(result);
            Assert.True(result.IsAvailable);
            Assert.Equal("Hinzugefügter Titel", result.Title);
            Assert.Equal("Hinzugefügter Author", result.Author);
            Assert.Equal("111111111", result.ISBN);
        }

        [Fact]
        public async Task EditBookTest()
        {
            using var scope = _factory.Services.CreateScope();
            var library = scope.ServiceProvider.GetRequiredService<LibraryService>();

            var updatedBook = new BookEntity
            {
                Title = "Neuer Titel",
                Author = "Neue Autorin",
                ISBN = "1234567890",
                YearPublished = 2023,
                IsAvailable = true,
                BorrowedCount = 5
            };

            var result = library.EditBook(updatedBook, 10);

            Assert.NotNull(result);
            Assert.Equal("Neuer Titel", result.Title);
            Assert.Equal("Neue Autorin", result.Author);
            Assert.Equal("1234567890", result.ISBN);
        }

        [Fact]
        public async Task DeleteBookTest()
        {
            using var scope = _factory.Services.CreateScope();
            var library = scope.ServiceProvider.GetRequiredService<LibraryService>();

            var result = library.DeleteBook(14);

            Assert.NotNull(result);
            Assert.True(result);
        }

        //Tests with invalid values
        [Fact]
        public async Task AddBook_WithMissingFields_Throws()
        {
            using var scope = _factory.Services.CreateScope();
            var library = scope.ServiceProvider.GetRequiredService<LibraryService>();

            var book = new BookEntity
            {
                Title = null,
                Author = "Author",
                ISBN = null,
                YearPublished = 2020,
                IsAvailable = true
            };

            Assert.Throws<DbUpdateException>(() => library.AddBook(book));
        }

        [Fact]
        public async Task EditBook_NonexistentId_ReturnsNull()
        {
            using var scope = _factory.Services.CreateScope();
            var library = scope.ServiceProvider.GetRequiredService<LibraryService>();

            var book = new BookEntity
            {
                Title = "Titel123",
                Author = "Author",
                ISBN = "1111133333",
                YearPublished = 2020,
                IsAvailable = true
            };

            var result = library.EditBook(book, -1);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteBook_InvalidId_ReturnFalse()
        {
            using var scope = _factory.Services.CreateScope();
            var library = scope.ServiceProvider.GetRequiredService<LibraryService>();

            var result = library.DeleteBook(-1);

            Assert.False(result);
        }

        [Fact]
        public async Task AddBook_WithOverThirteenCharsInISBN_Validation()
        {
            using var scope = _factory.Services.CreateScope();
            var library = scope.ServiceProvider.GetRequiredService<LibraryService>();

            var book = new BookEntity
            {
                Title = null,
                Author = "Author",
                ISBN = "ISBN-12142251512353523525235235235",
                YearPublished = 2020,
                IsAvailable = true
            };

            Assert.Throws<DbUpdateException>(() => library.AddBook(book));
        }

        [Fact]
        public async Task AddBook_WithOverHundredCharsInTitle_Validation()
        {
            using var scope = _factory.Services.CreateScope();
            var library = scope.ServiceProvider.GetRequiredService<LibraryService>();

            var book = new BookEntity
            {
                Title = "fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff",
                Author = "Author",
                ISBN = "ISBN-1214225",
                YearPublished = 2020,
                IsAvailable = true
            };

            Assert.Throws<DbUpdateException>(() => library.AddBook(book));
        }

        [Fact]
        public async Task AddBook_WithOverFiftyCharsInAuthor_Validation()
        {
            using var scope = _factory.Services.CreateScope();
            var library = scope.ServiceProvider.GetRequiredService<LibraryService>();

            var book = new BookEntity
            {
                Title = "Test",
                Author = "Authoooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooor",
                ISBN = "ISBN-1214225",
                YearPublished = 2020,
                IsAvailable = true
            };

            Assert.Throws<DbUpdateException>(() => library.AddBook(book));
        }

        [Fact]
        public async Task DeleteBook_NonexistentId_ReturnsFalse()
        {
            using var scope = _factory.Services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<LibraryService>();

            var result = service.DeleteBook(9999);

            Assert.False(result);
        }
    }
}

