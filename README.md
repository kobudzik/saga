# Saga

Sample implementations of the Saga pattern in .NET:

- **Saga.Choreography** — choreography-based flow with MassTransit and RabbitMQ
- **Saga.Orchestrator** — orchestration-based flow with Azure Durable Functions

## Requirements

- .NET 8 SDK
- RabbitMQ for the choreography sample
- Azure Functions Core Tools for the orchestrator sample

## Repository layout

- `Saga.Choreography/`
  - `Order.API` — entry API for creating orders
  - `OrderService`, `StockService`, `PaymentService` — event-driven services
  - `MessageContracts` — shared commands and events
- `Saga.Orchestrator/`
  - `Saga.Orchestrator` — durable functions orchestration sample

## Running the choreography sample

RabbitMQ is configured for `amqp://localhost`.

Run the services from their project folders:

- `Saga.Choreography/Order.API/Order.API.csproj`
- `Saga.Choreography/OrderService/OrderService.csproj`
- `Saga.Choreography/StockService/StockService.csproj`
- `Saga.Choreography/PaymentService/PaymentService.csproj`

## Running the orchestrator sample

Run:

```bash
dotnet run --project Saga.Orchestrator/Saga.Orchestrator/Saga.Orchestrator.csproj
```

The local profile uses port `7255`.
