using Jumia.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Jumia.Mvc.Controllers
{
    public class AdminController : Controller
    {
        private readonly IOrderService orderService;

        public AdminController(IOrderService orderService) 
        {
            this.orderService = orderService;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DisplayOrders()
        {
            var ordersDto = await orderService.GetAllOrdersAsync();
            return View(ordersDto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int orderId, string status)
        {
            await orderService.UpdateOrderStatusAsync(orderId, status);
            return RedirectToAction("DisplayOrders","Admin");
        }


    }
}
