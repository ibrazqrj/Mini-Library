using System.ComponentModel.DataAnnotations;

namespace WASMLibrary.Models.Requests
{
    public class EditBookRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required!")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required!")]
        [StringLength(50, ErrorMessage = "Author cannot exceed 50 characters!")]
        public string Author { get; set; }

        [Required(ErrorMessage = "ISBN is required!")]
        [StringLength(13, ErrorMessage = "ISBN must be max. 13 digits.")]
        [RegularExpression(@"^\d{10}(\d{3})?$", ErrorMessage = "ISBN must be 10 or 13 digits.")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Release year is required!")]
        [Range(1, 2025, ErrorMessage = "Year must be between 1 and 2025")]
        public int YearPublished { get; set; }

        public bool IsAvailable { get; set; }
    }
}
