using SubscriptionManager.Application.Models.DTOs;
using SubscriptionManager.Application.Models.ViewModels;

namespace SubscriptionManager.Application.Interfaces
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
