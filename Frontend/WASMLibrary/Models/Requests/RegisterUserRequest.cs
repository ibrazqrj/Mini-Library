namespace WASMLibrary.Models.Requests
{
    public class RegisterUserRequest
    {
        public string UserName { get; set; }
        public string Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Postalcode { get; set; }
        public string City { get; set; }
    }
}
