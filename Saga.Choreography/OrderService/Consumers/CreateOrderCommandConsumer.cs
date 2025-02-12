using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MessageContracts.Commands;
using MessageContracts.Events;
using Order.API.Models;

namespace OrderService.Consumers;

public class CreateOrderCommandConsumer(IPublishEndpoint publishEndpoint) : IConsumer<ICreateOrderCommand>
{
    public async Task Consume(ConsumeContext<ICreateOrderCommand> context)
    {
        var message = context.Message;

        // Some validation ...

        // Create order with Pending status

        if (message.FailOn == EventType.OrderCreated)
        {
            throw new System.Exception("Simulated failure on OrderCreated event.");
        }

        await publishEndpoint.Publish<IOrderCreatedEvent>(new
        {
            UserId = message.UserId,
            OrderId = 1,
            Items = message.Items.Select(s => new
            {
                Id = s.ProductId,
                Quantity = s.Quantity
            }),
            TotalAmount = message.Items.Sum(s => s.Quantity * s.Price)
        });
    }
}
