using System.ComponentModel.DataAnnotations;

namespace SubscriptionManager.Models.DTOs;

    /// <summary>
    /// Data Transfer Object for Customer.
    /// </summary>
    public class CustomerDto
    {
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
}