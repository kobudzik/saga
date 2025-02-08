using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Saga.Orchestrator.Models;

namespace Saga.Orchestrator.Functions;

public class Inventory
{
    [Function("ReserveInventory")]
    public async Task ReserveInventory([ActivityTrigger] Order order, FunctionContext context)
    {
        context.GetLogger(nameof(ReserveInventory)).LogInformation($"++++++Reserving inventory for Order {order.OrderId}");
        await Task.Delay(1000); // Simulate some processing time
    }

    [Function("CompensateInventory")]
    public async Task CompensateInventory([ActivityTrigger] Order order, FunctionContext context)
    {
        context.GetLogger(nameof(CompensateInventory)).LogWarning($"-----Compensating inventory for Order {order.OrderId}");
        await Task.Delay(1000);
    }
}