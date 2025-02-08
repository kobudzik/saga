using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = FunctionsApplication.CreateBuilder(args);

// Add logging services
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();  // Log to the console (for development)
    // loggingBuilder.AddApplicationInsights("<InstrumentationKey>"); // Enable Application Insights if needed
});

// Configure Functions Web Application (This sets up the necessary framework for your function app)
builder.ConfigureFunctionsWebApplication();

// Add any other services or extensions here
// builder.ConfigureDurableExtension();  // For Durable Functions setup, if needed

// Build and run the application
var app = builder.Build();
app.Run();
