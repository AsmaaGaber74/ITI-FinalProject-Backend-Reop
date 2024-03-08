using Jumia.Application.Contract;
using Jumia.Context;
using Jumia.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia.Application.Repository
{
    public class OrderRepository : IOrderReposatory
    {
        private readonly JumiaContext _context;

        public OrderRepository(JumiaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.orders
                .Include(o => o.User) // Assuming `User` is the navigation property in `Order`
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await _context.orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = newStatus;
                await _context.SaveChangesAsync();
            }
        }
    }
}

