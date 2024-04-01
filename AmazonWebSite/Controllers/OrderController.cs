using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Jumia.Dtos.ViewModel.Order;
using Jumia.Application.Services;
using Jumia.Dtos.ResultView;
using Jumia.Dtos.ViewModel.Product;
using Jumia.Model;
namespace AmazonWebSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync([FromBody] Createorder createorder)
        {
            try
            {
                // Now, also pass createorder.AddressId to the service method
                var result = await _orderService.CreateOrderAsync(createorder.orderQuantities, createorder.UserID, createorder.AddressId);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //// GET: api/Order
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var order = await _orderService.GetAllOrdersAsync();
                return Ok(order);
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        /// DELETE: api/Order/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                return Ok("Order deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrderProduct(UpdateOrderProductDto updateOrderProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _orderService.UpdateOrderProductAsync(updateOrderProductDto);

            if (result.IsSuccess)
            {
                return Ok(result.Entity);
            }

            return BadRequest(new { message = result.Message });
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(string userId)
        {
            try
            {
                var orders = await _orderService.GetOrdersByUserId(userId);
                if (orders == null)
                {
                    return NotFound();
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("orderid")]
        public async Task<IActionResult> GetOrderdetailsByUserId(int orderId)
        {
            try
            {
                var orders = await _orderService.GetOrderDetailsByorderId(orderId);
                if (orders == null)
                {
                    return NotFound();
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error");
            }
        }
    }
}
