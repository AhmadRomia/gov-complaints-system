using System.Net;
using System.Text.Json;
using Application.Common.Exceptions;
using FluentValidation;


namespace WebApi.Middlewares
{
    public class ErrorHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation error: {Message}", ex.Message);

                await WriteErrorResponse(context, StatusCodes.Status400BadRequest, ex.Message,
                    ex.Errors.Select(e => e.ErrorMessage).ToList());
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning("BadRequest: {Message}", ex.Message);

                await WriteErrorResponse(context, StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (ConflictException ex)
            {
                _logger.LogWarning("Conflict: {Message}", ex.Message);

                await WriteErrorResponse(context, StatusCodes.Status409Conflict, ex.Message, new List<string> { "409" });
            }
            //catch (NotFoundException ex)
            //{
            //    _logger.LogWarning("NotFound: {Message}", ex.Message);

            //    await WriteErrorResponse(context, StatusCodes.Status404NotFound, ex.Message);
            //}
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error");

                await WriteErrorResponse(context, StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred");
            }
        }

        private async Task WriteErrorResponse(HttpContext context, int statusCode, string message, List<string>? errors = null)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                success = false,
                message,
                errors
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }

}
