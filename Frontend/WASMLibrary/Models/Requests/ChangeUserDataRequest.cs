using System.ComponentModel.DataAnnotations;

namespace WASMLibrary.Models.Requests
{
    public class ChangeUserDataRequest
    {
        [Required(ErrorMessage = "Username is required!")]
        [StringLength(20, ErrorMessage = "Username cannot exceed 20 characters!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Phone number is required!")]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters!")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required!")]
        [StringLength(100, ErrorMessage = "Address cannot exceed 100 characters!")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Postal code is required!")]
        [StringLength(10, ErrorMessage = "Postal code cannot exceed 10 characters!")]
        public string Postalcode { get; set; }

        [Required(ErrorMessage = "City is required!")]
        [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters!")]
        public string City { get; set; }
    }
}
