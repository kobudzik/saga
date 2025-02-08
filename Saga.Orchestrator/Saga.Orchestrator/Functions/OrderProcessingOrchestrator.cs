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
        var log = context.CreateReplaySafeLogger("OrderProcessingOrchestrator");
        var order = context.GetInput<Order>();

        var sagaSteps = new List<SagaStep>
        {
            new() {
                Name = "ReserveInventory",
                Action = async ctx => await ctx.CallActivityAsync("ReserveInventory", order),
                Compensation = async ctx => await ctx.CallActivityAsync("CompensateInventory", order)
            },
            new() {
                Name = "ProcessPayment",
                Action = async ctx => await ctx.CallActivityAsync("ProcessPayment", order),
                Compensation = async ctx => await ctx.CallActivityAsync("CompensatePayment", order)
            },
            new() {
                Name = "ArrangeShipping",
                Action = async ctx => await ctx.CallActivityAsync("ArrangeShipping", order),
                Compensation = async ctx => await ctx.CallActivityAsync("CompensateShipping", order)
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
