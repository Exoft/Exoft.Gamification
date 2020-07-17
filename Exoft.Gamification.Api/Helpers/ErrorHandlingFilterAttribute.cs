using Exoft.Gamification.Api.Common.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Exoft.Gamification.Api.Helpers
{
    public class ErrorHandlingFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<ErrorHandlingFilterAttribute> _logger;

        public ErrorHandlingFilterAttribute
        (
            ILogger<ErrorHandlingFilterAttribute> logger
        )
        {
            _logger = logger;
        }

        public override async void OnException(ExceptionContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(
                $"\r\n ---> QueryString: {string.Join("\r\n\t -->", context.HttpContext.Request.Query)}");
            stringBuilder.Append($"\r\n ---> Params: {string.Join("\r\n\t -->", context.RouteData.Values)}");

            
            // if body
            context.HttpContext.Request.EnableBuffering();
            context.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);

            var jsonData = new JObject();
            using (var stream = new StreamReader(context.HttpContext.Request.Body))
            {
                var body = await stream.ReadToEndAsync();
                if(!string.IsNullOrEmpty(body))
                {
                    jsonData = JObject.Parse(body);
                }
            }

            // if form
            if (context.HttpContext.Request.HasFormContentType)
            {
                var form = context.HttpContext.Request.Form;
                jsonData = ToJSON(form);
            }
            
            if (jsonData.Count != 0)
            {
                var clearJson = RemoveExcludedProperties(jsonData, context);

                stringBuilder.Append($"\r\n ---> Data: {clearJson} \r\n");
            }

            _logger.LogError(context.Exception, stringBuilder.ToString());
        }

        private JObject RemoveExcludedProperties(JObject json, ExceptionContext context)
        {
            if (!(context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor))
            {
                return json;
            }

            var method = controllerActionDescriptor.MethodInfo;
            var parameter = method.GetParameters().SingleOrDefault(param =>
                param.IsDefined(typeof(FromBodyAttribute), false) ||
                param.IsDefined(typeof(FromFormAttribute), false));

            if (parameter == null)
            {
                return json;
            }

            var modelType = parameter.ParameterType;
            var excludedProperties = modelType.GetProperties().Where(prop =>
                IsDefined(prop, typeof(NonLoggedAttribute)));

            var propertyForExcludes = excludedProperties as PropertyInfo[] ?? excludedProperties.ToArray();
            if(!propertyForExcludes.Any())
            {
                return json;
            }

            foreach (var propertyForExclude in propertyForExcludes)
            {
                var jToken = json.GetValue(propertyForExclude.Name, System.StringComparison.OrdinalIgnoreCase);

                if(jToken != null)
                {
                    json.Remove(jToken.Path);
                }
            }

            return json;
        }
        
        private JObject ToJSON(IFormCollection formCollection)
        {
            var dict = formCollection.ToDictionary(s => s.Key, s => s.Value);

            return JObject.Parse(JsonConvert.SerializeObject(dict));
        }
    }
}
