using MassTransit;
using MessageContracts;
using Microsoft.AspNetCore.Builder;
using StockService.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<OrderCreatedEventConsumer>();
    config.AddConsumer<PaymentRejectedEventConsumer>();

    config.UsingRabbitMq((context, rabbitConfig) =>
    {
        rabbitConfig.Host(RabbitMQConstants.Uri);

        rabbitConfig.ReceiveEndpoint(RabbitMQConstants.StockOrderCreatedQueueName, e =>
        {
            e.ConfigureConsumer<OrderCreatedEventConsumer>(context);
        });

        rabbitConfig.ReceiveEndpoint(RabbitMQConstants.StockPaymentRejectedQueueName, e =>
        {
            e.ConfigureConsumer<PaymentRejectedEventConsumer>(context);
        });
    });
});
builder.Services.AddMassTransitHostedService();

var app = builder.Build();

app.Run();
