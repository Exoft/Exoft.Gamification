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
            Message = exception.ToString();
        }

        public string Message { get; set; }
    }
}
