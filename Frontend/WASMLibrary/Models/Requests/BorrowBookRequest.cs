using System.ComponentModel.DataAnnotations;

namespace WASMLibrary.Models.Requests
{
    public class BorrowBookRequest
    {
        [Required(ErrorMessage = "The book Id is required!")]
        public int BookId { get; set; }
    }
}
