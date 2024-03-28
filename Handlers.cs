using System.Text.Json;
using OpenTelemetry.Trace;

namespace TracingWeb;

public static class Handlers
{
    private static readonly JsonSerializerOptions jsonOptions = new()
    {     
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
   
    public static async Task<IResult> HandleAllPosts(Tracer tracer, IHttpClientFactory httpFactory)
    {
        using var span = tracer.StartActiveSpan("all-posts");

        var result = Enumerable.Empty<Post>();
        using (tracer.StartActiveSpan("http-request"))
        {
            var httpClient = httpFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://jsonplaceholder.typicode.com/posts");
            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                span.AddEvent("Error while making http request.");
                return Results.Problem();
            }

            using (tracer.StartActiveSpan("serializing-response"))
            {  
                using var contentStream = await response.Content.ReadAsStreamAsync();
                result = await JsonSerializer.DeserializeAsync<IEnumerable<Post>>(contentStream, jsonOptions) ?? result;
            }
        }

        span.SetAttribute("post-count", result.Count());
          
        return Results.Ok(result);
    }
}
