using System.ComponentModel.DataAnnotations;

namespace sale.models
{
    public class TotalRevenue
    {
        [Key]
        public int Id { get; set; }
        public int Revenue { get; set; }

    }
}
