# Tracing Web

Tracing Web demonstrates how to trace information on ASP.NET Core Web Application with [Open Telemetry](https://opentelemetry.io/). It uses OpenTelemetry official .NET SDKs to export traces to [Jaeger](https://www.jaegertracing.io/) backend.

## Running the project

You can run this project by running the docker compose file.

```
docker compose up
```

This will spin up the Web Application, Jaeger UI and Jaeger Collector.

To see the tracing in action, you can produce and inspect traces by visiting `http://localhost:3000/posts`. 

Then, visit Jeager UI at `http://localhost:16686` and select "tracing-web" service.

You should see

<img width="800" alt="image" src="https://github.com/fabioluz/tracing-web/assets/5049361/55a0dd1a-1b3e-4e85-a20a-aa27877fa05d">

and

<img width="800" alt="image" src="https://github.com/fabioluz/tracing-web/assets/5049361/d91c6eab-92fd-4724-a523-74ff631ed04d">

