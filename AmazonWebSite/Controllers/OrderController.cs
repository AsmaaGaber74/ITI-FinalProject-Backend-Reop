using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Jumia.Dtos.ViewModel.Order;
using Jumia.Application.Services;
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
    }
}
