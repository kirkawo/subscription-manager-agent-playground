using System.ComponentModel.DataAnnotations;

namespace SubscriptionManager.Entities
{
    public class Subscription
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [StringLength(3)]
        public string Currency { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;

        [StringLength(50)]
        public string PaymentMethod { get; set; } = string.Empty;

        [Required]
        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }
    }
}
