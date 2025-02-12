namespace MessageContracts;

public enum EventType
{
    CreateOrder,
    OrderCreated,
    PaymentConfirmed,
    PaymentRejected,
    StockReserved,
    StockNotReserved
}
