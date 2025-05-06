    using System.Text;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;

    namespace WebSearchApi.Middleware
    {
        public class BasicAuthenticationMiddleware
        {
            private readonly RequestDelegate _next;
            private const string BasicScheme = "Basic ";
            private readonly IConfiguration _config;
            private readonly string _username;
            private readonly string _password;
        public BasicAuthenticationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _config = configuration;
            _username = _config["BasicAuth:Username"];
            _password = _config["BasicAuth:Password"];
        }

        public async Task InvokeAsync(HttpContext context)
            {
                // Skip authentication for Swagger and other specific routes
                if (context.Request.Path.StartsWithSegments("/swagger"))
                {
                    await _next(context);
                    return;
                }
                if (!context.Request.Headers.TryGetValue("Authorization", out StringValues authHeader))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Authorization header missing.");
                    return;
                }

                var authHeaderValue = authHeader.ToString();
                if (!authHeaderValue.StartsWith(BasicScheme, StringComparison.OrdinalIgnoreCase))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid authorization scheme.");
                    return;
                }

                var encodedCredentials = authHeaderValue.Substring(BasicScheme.Length).Trim();
                var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
                var credentials = decodedCredentials.Split(':', 2);

                if (credentials.Length != 2 || !IsAuthorized(credentials[0], credentials[1]))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid username or password.");
                    return;
                }

                await _next(context);
            }

            private bool IsAuthorized(string username, string password)
            {
                return username == _username && password == _password;
            }
        }
    }


