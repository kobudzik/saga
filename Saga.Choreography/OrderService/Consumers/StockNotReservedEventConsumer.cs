using System.Threading.Tasks;
using MassTransit;
using MessageContracts.Events;

namespace OrderService.Consumers;

public class StockNotReservedEventConsumer : IConsumer<IStockNotReservedEvent>
{
    public Task Consume(ConsumeContext<IStockNotReservedEvent> context)
    {
        // Update order status from Pending to Rejected via {context.Message.OrderId}
        return Task.CompletedTask;
    }
}