using Microsoft.EntityFrameworkCore;
using SubscriptionManager.Data;
using SubscriptionManager.Entities;
using SubscriptionManager.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SubscriptionManager.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> CreateAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Customers.FindAsync(id);
            if (entity != null)
            {
                _context.Customers.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers
                .AsNoTracking()
                .Include(c => c.Subscriptions)
                .ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers
                .AsNoTracking()
                .Include(c => c.Subscriptions)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }
    }
}
