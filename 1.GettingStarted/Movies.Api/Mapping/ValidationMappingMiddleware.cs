using Movies.Contracts.Responses;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

namespace Movies.Api.Mapping
{
    public class ValidationMappingMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationMappingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                throw;
                //context.Response.StatusCode = 400;
                //var validatorFailureResponse = new ValidationFailureResponse
                //{
                //    Errors = ex.Errors.Select(x => new ValidatioResponse
                //    {
                //        PropertyName = x.PropertyName,
                //        Message = x.ErrorMessage
                //    })
                //};
                
                //await context.Response.WriteAsJsonAsync(validatorFailureResponse, cancellationToken: context.RequestAborted);

            }
        }
    }
}
