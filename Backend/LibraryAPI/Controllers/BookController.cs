using LibraryAPI.Services;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Azure.Core;
using AutoMapper;
using Swashbuckle.AspNetCore.Annotations;
using LibraryAPI.Data;
using Microsoft.AspNetCore.Authorization;

namespace LibraryAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly LibraryService _libraryService;
        private readonly IMapper _mapper;

        public BookController(LibraryService libraryService, IMapper mapper)
        {
            _libraryService = libraryService;
            _mapper = mapper;
        }

        [HttpGet("GetBooks")]
        [SwaggerOperation(
            Summary = "Get all books",
            Description = "Returns a list of all books",
            OperationId = "GetAllBooks"
            )]
        [SwaggerResponse(200, "Books found", typeof(IEnumerable<Book>))]
        public ActionResult<IEnumerable<Book>> GetBooks()
        {
            var books = _libraryService.ListBooks();
            return Ok(_mapper.Map<IEnumerable<Book>>(books));
        }

        [HttpGet("GetBookById/{id}")]
        [SwaggerOperation(
            Summary = "Get a book by the id",
            Description = "Returns a book",
            OperationId = "GetBookById"
        )]
        [SwaggerResponse(200, "Book found", typeof(Book))]
        public ActionResult<Book> GetBookById(int id)
        {
            var book = _libraryService.GetBookById(id);
            if (book == null)
                return NotFound("Book not found.");
            return Ok(_mapper.Map<Book>(book));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddBook")]
        [SwaggerOperation(
            Summary = "Add a book",
            Description = "Adds a book to the book list.",
            OperationId = "AddBook"
            )]
        [SwaggerResponse(200, "Book added", typeof(BookEntity))]
        public ActionResult<BookEntity> AddBook([FromBody] BookForCreate request)
        {
            var createdBook = _libraryService.AddBook(_mapper.Map<BookEntity>(request));

            return Ok(_mapper.Map<Book>(createdBook));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("EditBook/{id}")]
        [SwaggerOperation(
            Summary = "Edit a book",
            Description = "Edit an already added book",
            OperationId = "EditBook"
        )]
        [SwaggerResponse(200, "Book updated", typeof(Book))]
        public ActionResult<Book> EditBook(int id, [FromBody] BookForEdit request)
        {
            var updatedBook = _libraryService.EditBook(_mapper.Map<BookEntity>(request), id);

            if (updatedBook == null)
            {
                return NotFound("Book not found.");
            }

            return Ok(_mapper.Map<Book>(updatedBook));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteBook/{id}")]
        [SwaggerOperation(
            Summary = "Delete a book",
            Description = "Delete a book from the list",
            OperationId = "DeleteBook"
        )]
        [SwaggerResponse(200, "Book deleted", typeof(Book))]
        public ActionResult<Book> DeleteBook(int id)
        {
            var success = _libraryService.DeleteBook(id);

            if (success == false)
            {
                return NotFound("Book not found.");
            }

            return Ok("Book deleted!");
        }
    }
}
