using System.Threading.Tasks;
using MassTransit;
using MessageContracts;
using MessageContracts.Events;

namespace OrderService.Consumers;

public class PaymentConfirmedEventConsumer : IConsumer<IPaymentConfirmedEvent>
{
    public Task Consume(ConsumeContext<IPaymentConfirmedEvent> context)
    {
        var message = context.Message;

        if (message.FailOn == EventType.PaymentConfirmed)
            throw new System.Exception("Simulated failure on PaymentConfirmed event.");

        // Update order status from Pending to Confirmed via {context.Message.OrderId}
        return Task.CompletedTask;
    }
}
