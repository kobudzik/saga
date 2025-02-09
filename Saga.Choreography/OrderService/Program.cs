using MassTransit;
using MessageContracts;
using Microsoft.AspNetCore.Builder;
using OrderService.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<CreateOrderCommandConsumer>();
    config.AddConsumer<StockNotReservedEventConsumer>();
    config.AddConsumer<PaymentConfirmedEventConsumer>();
    config.AddConsumer<PaymentRejectedEventConsumer>();

    config.UsingRabbitMq((context, rabbitConfig) =>
    {
        rabbitConfig.Host(RabbitMQConstants.Uri);

        rabbitConfig.ReceiveEndpoint(RabbitMQConstants.CreateOrderQueueName, e =>
        {
            e.ConfigureConsumer<CreateOrderCommandConsumer>(context);
        });

        rabbitConfig.ReceiveEndpoint(RabbitMQConstants.OrderStockNotReservedQueueName, e =>
        {
            e.ConfigureConsumer<StockNotReservedEventConsumer>(context);
        });

        rabbitConfig.ReceiveEndpoint(RabbitMQConstants.PaymentConfirmedQueueName, e =>
        {
            e.ConfigureConsumer<PaymentConfirmedEventConsumer>(context);
        });

        rabbitConfig.ReceiveEndpoint(RabbitMQConstants.OrderPaymentRejectedQueueName, e =>
        {
            e.ConfigureConsumer<PaymentRejectedEventConsumer>(context);
        });
    });
});

builder.Services.AddMassTransitHostedService();

var app = builder.Build();

app.Run();
