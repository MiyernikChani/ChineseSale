using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChneseSaleApi.models
{
    public class Gift
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int DonatorId { get; set; }
        public int Price { get; set; }  
        public int? CountOfSales { get; set; }
        public string? Picture { get; set; }
        public bool Status { get; set; }//מתנה זמינה או לא
        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
