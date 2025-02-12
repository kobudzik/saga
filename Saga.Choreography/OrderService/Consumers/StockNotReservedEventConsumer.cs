using System.Threading.Tasks;
using MassTransit;
using MessageContracts.Events;
using Order.API.Models;

namespace OrderService.Consumers;

public class StockNotReservedEventConsumer : IConsumer<IStockNotReservedEvent>
{
    public Task Consume(ConsumeContext<IStockNotReservedEvent> context)
    {
        var message = context.Message;

        if (message.FailOn == EventType.StockNotReserved)
        {
            throw new System.Exception("Simulated failure on StockNotReserved event.");
        }

        // Update order status from Pending to Rejected via {context.Message.OrderId}
        return Task.CompletedTask;
    }
}
