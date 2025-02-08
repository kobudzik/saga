using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;
using Saga.Orchestrator.Models;

namespace Saga.Orchestrator.Functions;

public class OrderProcessingOrchestrator
{
    [Function("OrderProcessingOrchestrator")]
    public async Task<string> Run(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        var log = context.CreateReplaySafeLogger<OrderProcessingOrchestrator>();
        var order = context.GetInput<Order>();

        var sagaSteps = new List<SagaStep>
        {
            new() {
                Name = nameof(Inventory.ReserveInventory),
                Action = async ctx => await ctx.CallActivityAsync(nameof(Inventory.ReserveInventory), order),
                Compensation = async ctx => await ctx.CallActivityAsync(nameof(Inventory.CompensateInventory), order)
            },
            new() {
                Name = nameof(Payment.CompensatePayment),
                Action = async ctx => await ctx.CallActivityAsync(nameof(Payment.ProcessPayment), order),
                Compensation = async ctx => await ctx.CallActivityAsync(nameof(Payment.CompensatePayment), order)
            },
            new() {
                Name = nameof(Shipping.ArrangeShipping),
                Action = async ctx => await ctx.CallActivityAsync(nameof(Shipping.ArrangeShipping), order),
                Compensation = async ctx => await ctx.CallActivityAsync(nameof(Shipping.CompensateShipping), order)
            }
        };

        try
        {
            foreach (var step in sagaSteps)
            {
                await step.Action(context);
                step.IsCompleted = true;
            }

            return "Order processed successfully.";
        }
        catch (Exception ex)
        {
            log.LogError($"Saga failed: {ex.Message}. Starting compensations.");
            foreach (var step in sagaSteps.Where(s => s.IsCompleted))
            {
                await step.Compensation(context);
            }

            return "Order processing failed and compensation executed.";
        }
    }
}
