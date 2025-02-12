namespace Order.API.Models;

public enum EventType
{
    OrderCreated,
    PaymentConfirmed,
    PaymentRejected,
    StockReserved,
    StockNotReserved
}
