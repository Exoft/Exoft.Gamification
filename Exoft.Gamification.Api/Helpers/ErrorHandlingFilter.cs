using log4net;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Exoft.Gamification.Api.Helpers
{
    public class ErrorHandlingFilter : ExceptionFilterAttribute
    {
        private static ILog _logger = LogManager.GetLogger(typeof(ErrorHandlingFilter));

        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            _logger.Error("Unhandled exceptions", exception);
        }
    }
}
