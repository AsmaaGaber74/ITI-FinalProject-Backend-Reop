using Jumia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Jumia.Application.Contract
{
    public interface IOrderReposatory :IRepository<Order,int>
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task UpdateOrderStatusAsync(int orderId, string newStatus);
        Task DeleteOrderAsync(int orderId);
    }
}
