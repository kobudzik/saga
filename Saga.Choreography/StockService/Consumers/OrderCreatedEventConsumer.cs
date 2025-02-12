using System.Threading.Tasks;
using MassTransit;
using MessageContracts;
using MessageContracts.Events;

namespace StockService.Consumers;

public class OrderCreatedEventConsumer(IPublishEndpoint publishEndpoint) : IConsumer<IOrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<IOrderCreatedEvent> context)
    {
        var message = context.Message;

        if (message.FailOn == EventType.OrderCreated)
            throw new System.Exception("Simulated failure on OrderCreated event.");

        var stockResult = true;

        if (stockResult)
        {
            await publishEndpoint.Publish<IStockReservedEvent>(new
            {
                UserId = message.UserId,
                OrderId = message.OrderId,
                TotalAmount = message.TotalAmount,
                Items = message.Items
            });
        }
        else
        {
            await publishEndpoint.Publish<IStockNotReservedEvent>(new
            {
                OrderId = message.OrderId,
                Message = "Insufficient Stock",
                FailOn = message.FailOn
            });
        }
    }
}