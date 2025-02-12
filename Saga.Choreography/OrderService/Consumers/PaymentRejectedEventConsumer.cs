using System.Threading.Tasks;
using MassTransit;
using MessageContracts.Events;
using Order.API.Models;

namespace OrderService.Consumers;

public class PaymentRejectedEventConsumer : IConsumer<IPaymentRejectedEvent>
{
    public Task Consume(ConsumeContext<IPaymentRejectedEvent> context)
    {
        var message = context.Message;

        if (message.FailOn == EventType.PaymentRejected)
        {
            throw new System.Exception("Simulated failure on PaymentRejected event.");
        }

        // Update order status from Pending to Rejected via {context.Message.OrderId}
        return Task.CompletedTask;
    }
}
