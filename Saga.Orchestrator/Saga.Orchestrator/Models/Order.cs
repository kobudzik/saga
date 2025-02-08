namespace Saga.Orchestrator.Models;

public class Order
{
    public string OrderId { get; set; }
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public SimulatedExceptionFor? ExceptionFor { get; set; } // P6e58
}
