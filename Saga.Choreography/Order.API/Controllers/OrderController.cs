using System;
using System.Threading.Tasks;
using MassTransit;
using MessageContracts;
using MessageContracts.Commands;
using Microsoft.AspNetCore.Mvc;
using Order.API.Models;

namespace Order.API.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController(ISendEndpointProvider sendEndpointProvider) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(CreateOrderRequest createOrderRequest)
    {
        var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQConstants.CreateOrderQueueName}"));
        await sendEndpoint.Send<ICreateOrderCommand>(new
        {
            UserId = createOrderRequest.UserId,
            Items = createOrderRequest.Items
        });

        return Accepted();
    }
}
