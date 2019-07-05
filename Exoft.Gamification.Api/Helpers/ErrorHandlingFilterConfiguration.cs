using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Exoft.Gamification.Api.Helpers
{
    public class ErrorHandlingFilterConfiguration : IConfigureOptions<MvcOptions>
    {
        private readonly ILogger<ErrorHandlingFilter> _logger;

        public ErrorHandlingFilterConfiguration(ILogger<ErrorHandlingFilter> logger)  
        {
            _logger = logger;
        }

        public void Configure(MvcOptions options)
        {
            options.Filters.Add(new ErrorHandlingFilter(_logger));
        }
    }
}
