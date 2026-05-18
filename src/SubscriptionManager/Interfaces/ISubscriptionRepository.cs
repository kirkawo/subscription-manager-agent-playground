using System.Collections.Generic;
using System.Threading.Tasks;
using SubscriptionManager.Entities;

namespace SubscriptionManager.Interfaces
{
    public interface ISubscriptionRepository
    {
        Task<IEnumerable<Subscription>> GetAllAsync();
        Task<Subscription?> GetByIdAsync(int id);
        Task<Subscription> CreateAsync(Subscription subscription);
        Task UpdateAsync(Subscription subscription);
        Task DeleteAsync(int id);
    }
}
