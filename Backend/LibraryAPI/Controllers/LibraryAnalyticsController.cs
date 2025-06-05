using LibraryAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace LibraryAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryAnalyticsController : ControllerBase
    {
        private readonly LibraryAnalyticsService _libraryAnalyticsService;

        public LibraryAnalyticsController(LibraryAnalyticsService libraryAnalyticsService)
        {
            _libraryAnalyticsService = libraryAnalyticsService;
        }

        [HttpGet("MostBorrowedBookTitle")]
        [SwaggerOperation(
            Summary = "Shows statistic",
            Description = "Shows the most borrowed book's title.",
            OperationId = "ShowMostBorrowedBookTitle"
            )]
        [SwaggerResponse(200, "User logged in.", typeof(IActionResult))]
        public IActionResult ShowMostBorrowedBookTitle()
        {
            var analytic = _libraryAnalyticsService.AnalyzeMostBorrowedBookTitle();
            return Ok(analytic);
        }

        [HttpGet("MostBorrowedBookTimes")]
        [SwaggerOperation(
            Summary = "Shows statistic",
            Description = "Shows how many times the most borrowed book got borrowed.",
            OperationId = "ShowMostBorrowedBookTimes"
            )]
        [SwaggerResponse(200, "User logged in.", typeof(IActionResult))]
        public IActionResult ShowMostBorrowedBookTimes()
        {
            var analytic = _libraryAnalyticsService.AnalyzeMostBorrowedBookTimes();
            return Ok(analytic);
        }

        [HttpGet("TotalUserAmount")]
        [SwaggerOperation(
            Summary = "Shows statistic",
            Description = "Shows the total amount of registred users.",
            OperationId = "ShowTotalUserAmount"
            )]
        [SwaggerResponse(200, "User logged in.", typeof(IActionResult))]
        public IActionResult ShowTotalUserAmount()
        {
            var analytic = _libraryAnalyticsService.AnalyzeTotalUserAmount();
            return Ok(analytic);
        }

        [HttpGet("CurrentlyBorrowedBooks")]
        [SwaggerOperation(
            Summary = "Shows statistic",
            Description = "Shows the actual amount of borrowed books.",
            OperationId = "ShowAmountCurrentlyBorrowedBooks"
            )]
        [SwaggerResponse(200, "User logged in.", typeof(IActionResult))]
        public IActionResult ShowAmountCurrentlyBorrowedBooks()
        {
            var analytic = _libraryAnalyticsService.AnalyzeCurrentlyBorrowed();
            return Ok(analytic);
        }

        [HttpGet("TotalBorrowedBooks")]
        [SwaggerOperation(
            Summary = "Shows statistic",
            Description = "Shows the total how often books got borrowed and the actual amount.",
            OperationId = "ShowTotalBorrowedBooks"
            )]
        [SwaggerResponse(200, "User logged in.", typeof(IActionResult))]
        public IActionResult ShowTotalBorrowedBooks()
        {
            var analytic = _libraryAnalyticsService.AnalyzeTotalBorrowedBooks();
            return Ok(analytic);
        }
    }
}
