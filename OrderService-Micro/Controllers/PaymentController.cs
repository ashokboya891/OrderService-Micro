using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService_Micro.Models;

namespace OrderService_Micro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpPost("process")]
        public IActionResult ProcessPayment([FromBody] PaymentRequest request)
        {
            return request.Amount > 0
                ? Ok(new { Status = "Success", TransactionId = Guid.NewGuid() })
                : BadRequest("Invalid Payment");
        }
    }
}
