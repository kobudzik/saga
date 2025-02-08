namespace Saga.Orchestrator.Models;

public class Order
{
    public required string OrderId { get; set; }
    public required string ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public SimulatedExceptionFor? ExceptionFor { get; set; }
}
