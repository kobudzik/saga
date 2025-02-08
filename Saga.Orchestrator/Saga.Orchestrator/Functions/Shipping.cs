using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Saga.Orchestrator.Models;

namespace Saga.Orchestrator.Functions;

public class Shipping
{
    [Function("ArrangeShipping")]
    public async Task ArrangeShipping([ActivityTrigger] Order order, FunctionContext context)
    {
        var logger = context.GetLogger<Shipping>();
        logger.LogInformation("++++++Arranging shipping for Order {OrderId}", order.OrderId);

        if (order.ExceptionFor == SimulatedExceptionFor.Shipping)
            throw new Exception("Simulated exception for shipping.");

        await Task.Delay(1000);
    }

    [Function("CompensateShipping")]
    public async Task CompensateShipping([ActivityTrigger] Order order, FunctionContext context)
    {
        var logger = context.GetLogger<Shipping>();
        logger.LogWarning("-----Canceling shipping for Order {OrderId}", order.OrderId);
        await Task.Delay(1000);
    }
}
