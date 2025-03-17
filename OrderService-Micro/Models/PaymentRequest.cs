namespace OrderService_Micro.Models
{
    public class PaymentRequest
    {
        public string OrderId { get; set; }
        public double Amount { get; set; }
    }
}
