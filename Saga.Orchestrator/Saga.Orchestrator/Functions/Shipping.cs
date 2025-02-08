using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Saga.Orchestrator.Models;

namespace Saga.Orchestrator.Functions;

public class Shipping
{
    [Function("ArrangeShipping")]
    public async Task ArrangeShipping([ActivityTrigger] Order order, FunctionContext context)
    {
        context.GetLogger("ArrangeShipping").LogInformation($"++++++Arranging shipping for Order {order.OrderId}");
        await Task.Delay(1000);
    }

    [Function("CompensateShipping")]
    public async Task CompensateShipping([ActivityTrigger] Order order, FunctionContext context)
    {
        context.GetLogger("CompensateShipping").LogWarning($"-----Canceling shipping for Order {order.OrderId}");
        await Task.Delay(1000);
    }
}