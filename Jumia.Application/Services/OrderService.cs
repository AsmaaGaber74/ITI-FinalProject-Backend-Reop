using AutoMapper;
using Jumia.Context;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jumia.Application.Contract;
using Jumia.Dtos.ViewModel.Order;
using Jumia.Dtos.ResultView;
using Jumia.Model;
using Jumia.Dtos;
using System.Data.Entity;
using Jumia.Dtos.ViewModel.category;
using Jumia.Dtos.ViewModel.Product;

namespace Jumia.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderReposatory _orderRepository;
        private readonly IProductService _productService;
        private readonly IOrderProuduct _orderProuduct;
        private readonly IMapper _mapper;



        public OrderService(IOrderReposatory orderRepository, IMapper mapper, IProductService productService, IOrderProuduct orderProuduct)
        {
            _orderRepository = orderRepository;
            _productService = productService;
            _orderProuduct = orderProuduct;
            _mapper = mapper;

        }

        public async Task<ResultView<OrderDto>> CreateOrderAsync(List<OrderQuantity> ProdactID, String UserID)
        {
            try
            {


                decimal totalPrice = 0;
                foreach (var orderProductDto in ProdactID)
                {
                    var product = await _productService.GetOne(orderProductDto.ProductID);
                    if (product != null && product.Price >= 0)
                    {

                        totalPrice += product.Price * orderProductDto.Quantity;

                    }
                }
                string status = "Pending";
                DateTime datePlaced = DateTime.Now;
                var order = new Order
                {
                    DatePlaced = datePlaced,
                    TotalPrice = totalPrice,
                    Status = status,
                    UserID = UserID,


                };
                var newprd = await _orderRepository.CreateAsync(order);
                await _orderRepository.SaveChangesAsync();

                foreach (var id in ProdactID)
                {
                    var NewOrderPrd = _orderProuduct.CreateAsync(new OrderProduct
                    {
                        ProductId = id.ProductID,
                        OrderId = newprd.Id,
                        TotalPrice = totalPrice,
                        Quantity = id.Quantity,


                    });
                }

                await _orderProuduct.SaveChangesAsync();

                var createdOrderDto = _mapper.Map<OrderDto>(order);

                return new ResultView<OrderDto>
                {
                    IsSuccess = true,
                    Message = "Order created successfully",
                    Entity = createdOrderDto
                };
            }
            catch (Exception ex)
            {
                return new ResultView<OrderDto>
                {
                    IsSuccess = false,
                    Message = "Failed to create order: " + ex.Message
                };
            }
        }



        public async Task<ResultView<OrderProducutDTo>> UpdateOrderProductAsync(int orderId, int quantity)
        {
            try
            {
                var orderProduct = await _orderProuduct.GetByIdAsync(orderId);
                if (orderProduct == null)
                {
                    return new ResultView<OrderProducutDTo>
                    {
                        IsSuccess = false,
                        Message = $"Order id with ID {orderId} not found."
                    };
                }

                orderProduct.Quantity = quantity;

                var updatedOrder = await _orderProuduct.UpdateAsync(orderProduct);
                await _orderProuduct.SaveChangesAsync();
                var updatedOrderProductDto = _mapper.Map<OrderProducutDTo>(orderProduct);

                return new ResultView<OrderProducutDTo>
                {
                    IsSuccess = true,
                    Message = "Order product updated successfully.",
                    Entity = updatedOrderProductDto
                };
            }
            catch (Exception ex)
            {
                return new ResultView<OrderProducutDTo>
                {
                    IsSuccess = false,
                    Message = "Failed to update order product: " + ex.Message
                };
            }
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

        public async Task DeleteOrderAsync(int orderId)
        {
            await _orderRepository.DeleteOrderAsync(orderId);
        }



        public async Task<IEnumerable<OrderDto>> GetOrdersByUserId(string userId)
        {
            var orders = await _orderRepository.GetOrdersByUserId(userId);
            var orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return orderDtos;
        }


    }
}
