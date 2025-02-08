using Microsoft.DurableTask;

namespace Saga.Orchestrator.Models;

public class SagaStep
{
    public string Name { get; set; }
    public Func<TaskOrchestrationContext, Task> Action { get; set; }
    public Func<TaskOrchestrationContext, Task> Compensation { get; set; }
    public bool IsCompleted { get; set; } = false;
}
