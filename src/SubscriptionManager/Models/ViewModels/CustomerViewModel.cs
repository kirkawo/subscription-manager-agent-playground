using System.ComponentModel.DataAnnotations;

namespace SubscriptionManager.Models.ViewModels;

/// <summary>
/// View Model for Customer.
/// </summary>
public class CustomerViewModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the customer.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the customer.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address of the customer.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the phone number of the customer.
    /// </summary>
    [StringLength(20)]
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the address of the customer.
    /// </summary>
    [StringLength(200)]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of subscriptions for the customer.
    /// </summary>
    public int SubscriptionCount { get; set; }
}