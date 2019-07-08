using Exoft.Gamification.Api.Common.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            stringBuilder.Append(string.Format("\r\n ---> QueryString: {0}", 
                string.Join("\r\n\t -->", context.HttpContext.Request.Query)));
            stringBuilder.Append(string.Format("\r\n ---> Params: {0}",
                string.Join("\r\n\t -->", context.RouteData.Values)));

            
            // if body
            context.HttpContext.Request.EnableRewind();
            context.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);

            using (StreamReader stream = new StreamReader(context.HttpContext.Request.Body))
            {
                string body = stream.ReadToEnd();
                if(!string.IsNullOrEmpty(body))
                {
                    var jsonBody = JObject.Parse(body);

                    var clearJson = RemoveExcludedProperties(jsonBody, context);
                    
                    stringBuilder.Append(string.Format("\r\n ---> Body: {0} \r\n", clearJson.ToString()));
                }
            }
            
            // if form
            if(context.HttpContext.Request.HasFormContentType)
            {
                var form = context.HttpContext.Request.Form;
                var jsonForm = ToJSON(form);

                var clearJson = RemoveExcludedProperties(jsonForm, context);

                stringBuilder.Append(string.Format("\r\n ---> Form: {0} \r\n", clearJson.ToString()));
            }

            _logger.LogError(context.Exception, stringBuilder.ToString());
        }

        private JObject RemoveExcludedProperties(JObject json, ExceptionContext context)
        {
            var controllerName = context.RouteData.Values["controller"].ToString() + "Controller";
            var controllerType = Type.GetType("Exoft.Gamification.Api.Controllers." + controllerName);
            var actionName = context.RouteData.Values["action"].ToString();
            var method = controllerType.GetMethod(actionName);
            var parameters = method.GetParameters()[0];
            var modelType = parameters.ParameterType;
            var excludedProperties = modelType.GetProperties().Where(prop =>
                                        IsDefined(prop, typeof(NonLoggedAttribute)));
            
            foreach (var propertyForExlude in excludedProperties)
            {
                json.Remove(propertyForExlude.Name.ToLower());
            }

            return json;
        }
        
        private JObject ToJSON(IFormCollection formCollection)
        {
            var dictionary = new Dictionary<string, string>();
            foreach (string key in formCollection.Keys)
            {
                dictionary.Add(key, formCollection[key]);
            }

            return JObject.Parse(JsonConvert.SerializeObject(dictionary));
        }
    }
}
