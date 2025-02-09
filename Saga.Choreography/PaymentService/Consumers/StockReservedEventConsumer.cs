using System.Threading.Tasks;
using MassTransit;
using MessageContracts.Events;

namespace PaymentService.Consumers;

public class StockReservedEventConsumer(IPublishEndpoint publishEndpoint) : IConsumer<IStockReservedEvent>
{
    public async Task Consume(ConsumeContext<IStockReservedEvent> context)
    {
        var message = context.Message;

        var paymentResult = false; //HandlePayment

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