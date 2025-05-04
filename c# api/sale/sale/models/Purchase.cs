using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChneseSaleApi.models
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }
        public int GiftId { get; set; }
        public int CustomerId { get; set; }
        public bool Status { get; set; }//טיוטה או נרכש
        [Required]
        public int Ammount { get; set; }
        public int TotalPrice { get; set; }

        [ForeignKey("GiftId")]
        public Gift Gift { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
    }
}
