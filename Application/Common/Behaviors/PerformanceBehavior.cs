using MediatR;
using Microsoft.Extensions.Logging;

using System.Diagnostics;


namespace Application.Common.Behaviors
{
    public class PerformanceBehavior<TRequest, TResponse>
            : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly ILogger<TRequest> _logger;

        public PerformanceBehavior(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            var response = await next();

            stopwatch.Stop();

            var elapsedMs = stopwatch.ElapsedMilliseconds;

            if (elapsedMs > 500) 
            {
                _logger.LogWarning(
                    "Long Running Request: {RequestName} ({Elapsed} ms)",
                    typeof(TRequest).Name,
                    elapsedMs);
            }

            return response;
        }
    }
}
