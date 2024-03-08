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
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
            // If you choose to not use AutoMapper, you can manually map the entities to DTOs as done in your provided example.
        }

        public async Task UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            await _orderRepository.UpdateOrderStatusAsync(orderId, newStatus);
        }
    }
}

