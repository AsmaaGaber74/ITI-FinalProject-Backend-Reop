using AutoMapper;
using Jumia.Context;
using Jumia.Dtos;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia.Application.Services
{
    public class OrderService:IOrderService
    {
        private readonly JumiaContext jumiaContext;
        private readonly IMapper mapper;

        public OrderService(JumiaContext jumiaContext, IMapper mapper)
        {
            this.jumiaContext = jumiaContext;
            this.mapper = mapper;
        }

        //public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        //{
        //    var orders = await jumiaContext.orders.AsNoTracking().ToListAsync();
        //    return mapper.Map<IEnumerable<OrderDto>>(orders);
        //}
        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await jumiaContext.orders
                .Include(o => o.User) // Assuming `User` is the navigation property in `Order` for `ApplicationUser`
                .AsNoTracking()
                .ToListAsync();

            var ordersDto = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                UserID = o.User.Id,
                UserName=o.User.UserName,
                DatePlaced = o.DatePlaced,
                TotalPrice = o.TotalPrice,
                Status = o.Status
                
            }).ToList();

            return ordersDto;
        }

        public async Task UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await jumiaContext.orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = newStatus;
                await jumiaContext.SaveChangesAsync();
            }
        }
    }
}

