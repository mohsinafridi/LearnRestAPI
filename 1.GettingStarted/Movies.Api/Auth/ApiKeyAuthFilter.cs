using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Movies.Api.Auth
{
    public class ApiKeyAuthFilter : IAuthorizationFilter
    {
        
        private readonly IConfiguration _configuration;
        public ApiKeyAuthFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var apiKey = _configuration["ApiKey"]!;
            if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey) ||
                !string.Equals(extractedApiKey, apiKey, StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new UnauthorizedObjectResult("API Key is missing!");
                return;
            }                      
        }
    }
}
