using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviors
{
    public class ExceptionBehavior<TRequest, TResponse>
            : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly ILogger<TRequest> _logger;

        public ExceptionBehavior(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;

                _logger.LogError(ex,
                    "Unhandled exception for Request {RequestName}. Error: {Message}",
                    requestName, ex.Message);

                throw; // let GlobalExceptionMiddleware handle the response
            }
        }
    }
}
