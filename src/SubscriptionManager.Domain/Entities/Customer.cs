using System.ComponentModel.DataAnnotations;

namespace SubscriptionManager.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
