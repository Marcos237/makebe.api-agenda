using System.Text.Json;
using api.makebe.agenda.Configurations;
using Microsoft.Extensions.Options;

namespace api.makebe.agenda.applications.Meddleweres;

public sealed class ApiSecurityMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ApiSecurityOptions _options;

    public ApiSecurityMiddleware(RequestDelegate next, IOptions<ApiSecurityOptions> options)
    {
        _next = next;
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!_options.Enabled)
        {
            await _next(context);
            return;
        }

        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                message = "ApiSecurity.ApiKey nao configurada."
            }));
            return;
        }

        if (!context.Request.Headers.TryGetValue(_options.HeaderName, out var receivedApiKey) ||
            !string.Equals(receivedApiKey.ToString(), _options.ApiKey, StringComparison.Ordinal))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                message = "Header de seguranca invalido ou ausente."
            }));
            return;
        }

        await _next(context);
    }
}
