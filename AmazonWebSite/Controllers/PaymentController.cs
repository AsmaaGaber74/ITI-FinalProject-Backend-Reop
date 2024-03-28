using Jumia.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmazonWebSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentServices _paymentServices;

        public PaymentController(IPaymentServices paymentServices) 
        {
            _paymentServices = paymentServices;
        }
        [HttpPost]
        public IActionResult create(int orderid)
        {
            if (orderid == 0) 
            {
                return BadRequest("order not found");
            }
            var paymentcreated = _paymentServices.Create(orderid);
            return Ok(paymentcreated);
        }
    }
}
