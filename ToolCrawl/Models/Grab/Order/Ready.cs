using System.ComponentModel.DataAnnotations;

namespace ToolCrawl.Models.Grab.Order
{
    public class Ready
    {
        [Key]
        [Required]
        public string OrderId { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        [Required]
        public string Items { get; set; }
        [Required]
        public string Driver { get; set; }
        [Required]
        public string PickupIn { get; set; }
        [Required]
        public string Receipt { get; set; }

    }
}
