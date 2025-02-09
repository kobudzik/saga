using MassTransit;
using MessageContracts;
using Microsoft.AspNetCore.Builder;
using PaymentService.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<StockReservedEventConsumer>();

    config.UsingRabbitMq((context, rabbitConfig) =>
    {
        rabbitConfig.Host(RabbitMQConstants.Uri);

        rabbitConfig.ReceiveEndpoint(RabbitMQConstants.PaymentStockReservedQueue, e =>
        {
            e.ConfigureConsumer<StockReservedEventConsumer>(context);
        });
    });
});
builder.Services.AddMassTransitHostedService();

var app = builder.Build();

app.Run();
