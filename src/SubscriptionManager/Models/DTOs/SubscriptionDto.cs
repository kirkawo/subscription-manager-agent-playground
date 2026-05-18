using System.ComponentModel.DataAnnotations;

namespace SubscriptionManager.Models.DTOs;

/// <summary>
/// Data Transfer Object for Subscription.
/// </summary>
public class SubscriptionDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the subscription.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the subscription.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the subscription.
    /// </summary>
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the start date of the subscription.
    /// </summary>
    [Required]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the end date of the subscription.
    /// </summary>
    [Required]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Gets or sets the price of the subscription.
    /// </summary>
    [Required]
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the currency of the subscription.
    /// </summary>
    [Required]
    [StringLength(3)]
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the status of the subscription.
    /// </summary>
    [Required]
    [StringLength(20)]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the payment method for the subscription.
    /// </summary>
    [StringLength(50)]
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer identifier associated with the subscription.
    /// </summary>
    [Required]
    public int CustomerId { get; set; }
}