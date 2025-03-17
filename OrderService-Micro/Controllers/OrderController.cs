using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;

namespace OrderService_Micro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IConnectionMultiplexer _redis;

        public OrderController(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        [HttpPost("placeorder")]
        public async Task<IActionResult> PlaceOrder([FromBody] Order order)
        {
            var subscriber = _redis.GetSubscriber();
            var orderJson = JsonSerializer.Serialize(order);

            // Publish Order Created Event
            await subscriber.PublishAsync("orders_channel", orderJson);

            return Ok(new { message = "Order placed successfully!", order });
        }
    }

    public class Order
    {
        public string OrderId { get; set; } = Guid.NewGuid().ToString();
        public string CustomerName { get; set; }
        public string Product { get; set; }
        public decimal Price { get; set; }
    }
    //[HttpPost("placeorder")]
    //public async Task<IActionResult> PlaceOrder([FromBody] string orderDetails)
    //{
    //    var publisher = _redis.GetSubscriber();
    //    await publisher.PublishAsync("order_channel", orderDetails);
    //    return Ok(new { Message = "Order placed successfully!", Order = orderDetails });
    //}
}

