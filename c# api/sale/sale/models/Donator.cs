using System.ComponentModel.DataAnnotations;

namespace ChneseSaleApi.models
{
    public class Donator
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [MaxLength(10)]
        [Required]
        public string Phone { get; set; }
        [Required]
        [EmailAddress]
        public string Mail { get; set; }
        public string Address { get; set; }
        public ICollection<Gift>? Gifts { get; set; }
    }
}
