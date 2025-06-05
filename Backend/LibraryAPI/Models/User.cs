using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
    public class User
    {
        public string ObjectID { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string UserName { get; set; }
        [MaxLength(60)]
        public string EMail { get; set; }
        [MaxLength(20)]
        public string Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        [MaxLength(20)]
        public string Postalcode { get; set; }
        public string City { get; set; }
        public bool RegistrationComplete { get; set; }
    }
}
