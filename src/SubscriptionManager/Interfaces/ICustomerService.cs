using System.Collections.Generic;
using System.Threading.Tasks;
using SubscriptionManager.Models.DTOs;
using SubscriptionManager.Models.ViewModels;

namespace SubscriptionManager.Interfaces
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
