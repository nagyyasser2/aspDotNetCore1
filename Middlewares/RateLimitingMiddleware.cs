using System.Collections.Concurrent;

namespace aspDotNetCore.Middlewares
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ConcurrentDictionary<string, ClientRequestInfo> ClientRequests = new();
        private readonly int _requestLimit = 100; // Max number of requests allowed
        private readonly TimeSpan _timeWindow = TimeSpan.FromMinutes(1); // Time window for rate limiting

        public RateLimitingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var clientIdentifier = GetClientIdentifier(context);

            if (string.IsNullOrEmpty(clientIdentifier))
            {
                await _next(context);
                return;
            }

            var clientRequestInfo = ClientRequests.GetOrAdd(clientIdentifier, _ => new ClientRequestInfo
            {
                Timestamp = DateTime.UtcNow,
                RequestCount = 0
            });

            lock (clientRequestInfo)
            {
                // Check if the request count needs resetting
                if (DateTime.UtcNow - clientRequestInfo.Timestamp > _timeWindow)
                {
                    clientRequestInfo.Timestamp = DateTime.UtcNow;
                    clientRequestInfo.RequestCount = 0;
                }

                // Increment the request count
                clientRequestInfo.RequestCount++;
            }

            if (clientRequestInfo.RequestCount > _requestLimit)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("Too many requests. Please try again later.");
                return;
            }

            await _next(context);
        }

        private string GetClientIdentifier(HttpContext context)
        {
            // Use the client IP address as the identifier
            return context.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        }

        private class ClientRequestInfo
        {
            public DateTime Timestamp { get; set; }
            public int RequestCount { get; set; }
        }
    }
}
