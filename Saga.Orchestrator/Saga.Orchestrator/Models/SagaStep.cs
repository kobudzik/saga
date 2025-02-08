using Microsoft.DurableTask;

namespace Saga.Orchestrator.Models;

public class SagaStep
{
    public required string Name { get; set; }
    public required Func<TaskOrchestrationContext, Task> Action { get; set; }
    public required Func<TaskOrchestrationContext, Task> Compensation { get; set; }
    public bool IsCompleted { get; set; } = false;
}
