using AutoMapper;
using LibraryAPI.Data;
using LibraryAPI.Models;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LibraryAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowingController : Controller
    {
        private readonly BorrowingService _borrowingService;
        private readonly IMapper _mapper;

        public BorrowingController(BorrowingService borrowingService, IMapper mapper)
        {
            _borrowingService = borrowingService;
            _mapper = mapper;
        }

        [HttpPut("BorrowBook")]
        [SwaggerOperation(
           Summary = "Borrow a book",
           Description = "Sets the availability of the borrowed book to false.",
           OperationId = "BorrowBook"
           )]
        [SwaggerResponse(200, "Book borrowed", typeof(BorrowingsEntity))]
        public async Task<IActionResult> BorrowBook([FromBody] BookForBorrow request)
        {
            var oid = User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;

            var success = _borrowingService.BorrowBook(_mapper.Map<BorrowingsEntity>(request), oid);

            if (!string.IsNullOrEmpty(success) && !success.Contains("Book is currently unavailable."))
            {
                string messagePartOne = "Book borrowed successfully!";
                string messagePartTwo = $"{success}";

                string message = messagePartOne + "\r\n" + messagePartTwo;

                return Ok(message);
            }
            else
            {
                return BadRequest("The book couldn't be borrowed. It might be unavailable or doesn't exist.");
            }
        }

        [HttpPut("ReturnBook")]
        [SwaggerOperation(
            Summary = "Return a book",
            Description = "Sets the availability of the returned book to true.",
            OperationId = "ReturnBook"
            )]
        [SwaggerResponse(200, "Book returned", typeof(BorrowingsEntity))]
        public async Task<IActionResult> ReturnBook([FromBody] BookForReturn request)
        {
            var oid = User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;

            var success = _borrowingService.ReturnBook(_mapper.Map<BorrowingsEntity>(request), oid);

            if (!string.IsNullOrEmpty(success))
            {
                string messagePartOne = "Book returned successfully!";
                string messagePartTwo = $"{success}";

                string message = messagePartOne + "\r\n" + messagePartTwo;

                return Ok(message);
            }
            else
            {
                return BadRequest("The book couldn't be returned. It might be available or doesn't exist.");
            }
        }

        [HttpGet("GetBorrowedBooksByOid")]
        [SwaggerOperation(
            Summary = "Get borrowed books by current user",
            Description = "Returns a list of books the current user has borrowed",
            OperationId = "GetBorrowedBooksByOid"
        )]
        [SwaggerResponse(200, "Books found", typeof(IEnumerable<Book>))]
        public ActionResult<IEnumerable<Book>> GetBorrowedBooksByOid()
        {
            var oid = User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;

            var books = _borrowingService.GetBorrowedBooks(oid);
            return Ok(_mapper.Map<IEnumerable<Book>>(books));
        }
    }
}
