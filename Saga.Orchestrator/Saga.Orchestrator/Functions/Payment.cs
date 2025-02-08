using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Saga.Orchestrator.Models;
namespace Saga.Orchestrator.Functions;

public class Payment
{
    [Function("ProcessPayment")]
    public async Task ProcessPayment([ActivityTrigger] Order order, FunctionContext context)
    {
        context.GetLogger(nameof(ProcessPayment)).LogInformation($"++++++Processing payment for Order {order.OrderId}");

        if (order.ExceptionFor == SimulatedExceptionFor.Payment)
        {
            throw new Exception("Simulated exception for payment.");
        }

        await Task.Delay(1000);
    }

    [Function("CompensatePayment")]
    public async Task CompensatePayment([ActivityTrigger] Order order, FunctionContext context)
    {
        context.GetLogger(nameof(CompensatePayment)).LogWarning($"-----Refunding payment for Order {order.OrderId}");
        await Task.Delay(1000);
    }
}
