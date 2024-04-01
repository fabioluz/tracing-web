using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using static TracingWeb.Handlers;

var builder = WebApplication.CreateBuilder(args);

const string serviceName = "tracing-web";

builder
    .Services
    .AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(serviceName))
    .WithTracing(tracing => tracing
        .AddSource(serviceName)
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddOtlpExporter())
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddOtlpExporter());

builder
    .Logging
    .ClearProviders()
    .AddOpenTelemetry(options =>
    {
        var resourceBuilder = ResourceBuilder
            .CreateDefault()
            .AddService(serviceName);

        options
            .SetResourceBuilder(resourceBuilder)
            .AddConsoleExporter()
            .AddOtlpExporter();
    });

var tracer = TracerProvider
    .Default
    .GetTracer(serviceName);

builder
    .Services
    .AddSingleton(tracer);

builder
    .Services
    .AddHttpClient();

var port = Environment.GetEnvironmentVariable("PORT") ?? "3000";

builder
    .Build()
    .MapGet("/posts", HandleAllPosts)
    .Run($"http://localhost:{port}");
