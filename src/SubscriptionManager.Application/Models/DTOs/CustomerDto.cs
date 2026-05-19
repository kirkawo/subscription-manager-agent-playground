using System.ComponentModel.DataAnnotations;

namespace SubscriptionManager.Application.Models.DTOs;

public class CustomerDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [StringLength(20)]
    public string Phone { get; set; } = string.Empty;

    [StringLength(200)]
    public string Address { get; set; } = string.Empty;
}
