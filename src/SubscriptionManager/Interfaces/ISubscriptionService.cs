using System.Collections.Generic;
using System.Threading.Tasks;
using SubscriptionManager.Models.DTOs;
using SubscriptionManager.Models.ViewModels;

namespace SubscriptionManager.Interfaces
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<SubscriptionViewModel>> GetAllSubscriptionsAsync();
        Task<SubscriptionViewModel?> GetSubscriptionByIdAsync(int id);
        Task<SubscriptionViewModel> CreateSubscriptionAsync(SubscriptionDto dto);
        Task<SubscriptionViewModel> UpdateSubscriptionAsync(int id, SubscriptionDto dto);
        Task DeleteSubscriptionAsync(int id);
    }
}
