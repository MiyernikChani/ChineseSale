using System.ComponentModel.DataAnnotations;

namespace ChneseSaleApi.models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        [MaxLength(10)]
        public string Phone { get; set; }
        [Required]
        [EmailAddress]
        public string Mail { get; set; }
        public string Address { get; set; }
        public string Role { get; set; } = "user";
    }
}
