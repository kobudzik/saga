using System.Threading.Tasks;
using MassTransit;
using MessageContracts.Events;
using Order.API.Models;

namespace PaymentService.Consumers;

public class StockReservedEventConsumer(IPublishEndpoint publishEndpoint) : IConsumer<IStockReservedEvent>
{
    public async Task Consume(ConsumeContext<IStockReservedEvent> context)
    {
        var message = context.Message;

        if (message.FailOn == EventType.StockReserved)
        {
            throw new System.Exception("Simulated failure on StockReserved event.");
        }

        var paymentResult = true; //HandlePayment

        if (paymentResult)
        {
            await publishEndpoint.Publish<IPaymentConfirmedEvent>(new
            {
                OrderId = message.OrderId
            });
        }
        else
        {
            await publishEndpoint.Publish<IPaymentRejectedEvent>(new
            {
                UserId = message.UserId,
                OrderId = message.OrderId,
                Items = message.Items,
                Message = "Payment rejected."
            });
        }
    }
}
