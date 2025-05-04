using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChneseSaleApi.models
{
    public class Winner
    {
        [Key]
        public int Id { get; set; }
        public int GiftId { get; set; }
        public int CustomerId { get; set; }

        [ForeignKey("GiftId")]
        public Gift Gift { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
    }
}
