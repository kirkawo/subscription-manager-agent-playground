using SubscriptionManager.Application.Models.DTOs;
using SubscriptionManager.Application.Models.ViewModels;

namespace SubscriptionManager.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerViewModel>> GetAllCustomersAsync();
        Task<CustomerViewModel?> GetCustomerByIdAsync(int id);
        Task<CustomerViewModel> CreateCustomerAsync(CustomerDto dto);
        Task<CustomerViewModel> UpdateCustomerAsync(int id, CustomerDto dto);
        Task DeleteCustomerAsync(int id);
    }
}
