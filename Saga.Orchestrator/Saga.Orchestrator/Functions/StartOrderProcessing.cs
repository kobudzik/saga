using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using Saga.Orchestrator.Models;
using System.Net;
using System.Text.Json;

namespace Saga.Orchestrator.Functions;

public class StartOrderProcessing
{
    [Function("StartOrderProcessing")]
    public async Task<HttpResponseData> StartAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
        [DurableClient] DurableTaskClient starter,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("StartOrderProcessing");

        var order = await JsonSerializer.DeserializeAsync<Order>(req.Body, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (order == null)
        {
            var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await badRequestResponse.WriteStringAsync("Invalid request payload.");
            return badRequestResponse;
        }

        string instanceId = await starter.ScheduleNewOrchestrationInstanceAsync("OrderProcessingOrchestrator", order);

        logger.LogInformation($"Started orchestration with ID = '{instanceId}'.");

        var response = req.CreateResponse(HttpStatusCode.Accepted);
        await response.WriteStringAsync($"Orchestration started with ID = {instanceId}");
        return response;
    }
}
