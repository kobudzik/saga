using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using Saga.Orchestrator.Models;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saga.Orchestrator.Functions;

public class StartOrderProcessing
{
    private static readonly JsonSerializerOptions _serializationOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    [Function("StartOrderProcessing")]
    public async Task<HttpResponseData> StartAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData request,
        [DurableClient] DurableTaskClient starter,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger<StartOrderProcessing>();
        var order = await JsonSerializer.DeserializeAsync<Order>(request.Body, _serializationOptions);

        if (order == null)
        {
            var badRequestResponse = request.CreateResponse(HttpStatusCode.BadRequest);
            await badRequestResponse.WriteStringAsync("Invalid request payload.");
            return badRequestResponse;
        }

        string instanceId = await starter.ScheduleNewOrchestrationInstanceAsync("OrderProcessingOrchestrator", order);

        logger.LogInformation("Started orchestration with ID = '{InstanceId}'.", instanceId);

        var response = request.CreateResponse(HttpStatusCode.Accepted);
        await response.WriteStringAsync($"Orchestration started with ID = {instanceId}");
        return response;
    }
}
