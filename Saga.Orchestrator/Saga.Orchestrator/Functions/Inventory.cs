using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Saga.Orchestrator.Models;

namespace Saga.Orchestrator.Functions;

public class Inventory
{
    [Function("ReserveInventory")]
    public async Task ReserveInventory([ActivityTrigger] Order order, FunctionContext context)
    {
        var logger = context.GetLogger<Inventory>();
        logger.LogInformation("++++++Reserving inventory for Order {OrderId}", order.OrderId);

        if (order.ExceptionFor == SimulatedExceptionFor.Inventory)
            throw new Exception("Simulated exception for inventory.");

        await Task.Delay(1000);
    }

    [Function("CompensateInventory")]
    public async Task CompensateInventory([ActivityTrigger] Order order, FunctionContext context)
    {
        var logger = context.GetLogger<Inventory>();
        logger.LogWarning("-----Compensating inventory for Order {OrderId}", order.OrderId);
        await Task.Delay(1000);
    }
}
