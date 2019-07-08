using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;

namespace Exoft.Gamification.Api.Helpers
{
    public class ErrorHandlingFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ErrorHandlingFilter> _logger;

        public ErrorHandlingFilter
        (
            ILogger<ErrorHandlingFilter> logger
        )
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(string.Format("\r\n ---> Controller: {0}", context.RouteData.Values["controller"]));
            stringBuilder.Append(string.Format("\r\n ---> Action: {0}", context.RouteData.Values["action"]));
            stringBuilder.Append(string.Format("\r\n ---> QueryString: {0}", 
                string.Join("\r\n\t -->", context.HttpContext.Request.Query)));
            stringBuilder.Append(string.Format("\r\n ---> Params: {0}",
                string.Join("\r\n\t -->", context.RouteData.Values)));

            context.HttpContext.Request.EnableRewind();
            context.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);

            using (StreamReader stream = new StreamReader(context.HttpContext.Request.Body))
            {
                string body = stream.ReadToEnd();
                stringBuilder.Append(string.Format("\r\n ---> Body: {0}", body));
            }
            
            if(context.HttpContext.Request.HasFormContentType)
            {
                stringBuilder.Append(string.Format("\r\n ---> Form: {0} \r\n", string.Join("\r\n\t -->", 
                    context.HttpContext.Request.Form)));
            }

            _logger.LogError(context.Exception, stringBuilder.ToString());
        }
    }
}
