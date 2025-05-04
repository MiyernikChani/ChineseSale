using System.ComponentModel.DataAnnotations;

namespace sale.models
{
    public class Count
    {
        [Key]
        public int id { get; set; }
        public int count { get; set; }
    }
}
