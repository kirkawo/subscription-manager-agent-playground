using Microsoft.EntityFrameworkCore;
using SubscriptionManager.Application.Interfaces;
using SubscriptionManager.Domain.Entities;
using SubscriptionManager.Infrastructure.Data;

namespace SubscriptionManager.Infrastructure.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly AppDbContext _context;

        public SubscriptionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Subscription> CreateAsync(Subscription subscription)
        {
            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
            return subscription;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Subscriptions.FindAsync(id);
            if (entity != null)
            {
                _context.Subscriptions.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Subscription>> GetAllAsync()
        {
            return await _context.Subscriptions
                .AsNoTracking()
                .Include(s => s.Customer)
                .ToListAsync();
        }

        public async Task<Subscription?> GetByIdAsync(int id)
        {
            return await _context.Subscriptions
                .AsNoTracking()
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task UpdateAsync(Subscription subscription)
        {
            _context.Subscriptions.Update(subscription);
            await _context.SaveChangesAsync();
        }
    }
}
