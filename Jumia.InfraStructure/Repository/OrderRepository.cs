using Jumia.Application.Contract;
using Jumia.Context;
using Jumia.Model;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Jumia.InfraStructure.Repository
{
    public class OrderRepository : Repository<Order, int>, IOrderReposatory
    {
        private readonly JumiaContext context;

        public OrderRepository(JumiaContext context) : base(context) // Pass the context to the base class
        {
            this.context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await context.orders
                .Include(o => o.User) // Correct if "orders" is the correct DbSet name; otherwise, it should match your DbSet name exactly, case-sensitive
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await context.orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = newStatus;
                await context.SaveChangesAsync();
            }
        }
    }

}


