using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MessageContracts;
using MessageContracts.Commands;
using MessageContracts.Events;

namespace OrderService.Consumers;

public class CreateOrderCommandConsumer(IPublishEndpoint publishEndpoint) : IConsumer<ICreateOrderCommand>
{
    public async Task Consume(ConsumeContext<ICreateOrderCommand> context)
    {
        var message = context.Message;

        // Some validation ...

        // Create order with Pending status

        if (message.FailOn == EventType.CreateOrder)
            throw new System.Exception("Simulated failure on CreateOrder event.");

        await publishEndpoint.Publish<IOrderCreatedEvent>(new
        {
            UserId = message.UserId,
            OrderId = 1,
            Items = message.Items.Select(s => new
            {
                Id = s.ProductId,
                Quantity = s.Quantity
            }),
            TotalAmount = message.Items.Sum(s => s.Quantity * s.Price),
            FailOn = message.FailOn
        });
    }
}
