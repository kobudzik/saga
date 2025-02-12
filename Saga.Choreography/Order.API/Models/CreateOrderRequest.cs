using System.Collections.Generic;

namespace Order.API.Models;

public class CreateOrderRequest
{
    public int UserId { get; set; }
    public List<OrderItem> Items { get; set; }
    public EventType FailOn { get; set; }
}

public enum EventType
{
    OrderCreated,
    PaymentConfirmed,
    PaymentRejected,
    StockReserved,
    StockNotReserved
}
