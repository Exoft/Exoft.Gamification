using System;
using System.Collections.Generic;
using System.Text;

namespace Exoft.Gamification.Api.Common.Models
{
    public class ErrorResponseModel
    {
        public ErrorResponseModel(string message)
        {
            Message = message;
        }

        public ErrorResponseModel(Exception exception)
        {
            StringBuilder sb = new StringBuilder(exception.Message);

            while (exception.InnerException != null)
            {
                exception = exception.InnerException;

                sb.AppendLine($"--> {exception.Message}");
            }

            Message = sb.ToString();
        }

        public string Message { get; set; }
    }
}
