namespace MessageContracts.Events;

public interface IPaymentConfirmedEvent
{
    int OrderId { get; }
    EventType FailOn { get; }
}