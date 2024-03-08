using AutoMapper;
using Jumia.Context;
using Jumia.Dtos;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jumia.Application.Contract;

namespace Jumia.Application.Services
{
    public class OrderService:IOrderService
    {
        private readonly IOrderReposatory _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderReposatory orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            //return _mapper.Map<IEnumerable<OrderDto>>(orders);
            
            var ordersDto = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                UserID = o.User.Id,
                UserName = o.User.UserName,
                DatePlaced = o.DatePlaced,
                TotalPrice = o.TotalPrice,
                Status = o.Status

            }).ToList();

            return ordersDto;
        }

        public async Task UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            await _orderRepository.UpdateOrderStatusAsync(orderId, newStatus);
        }
    }
}

