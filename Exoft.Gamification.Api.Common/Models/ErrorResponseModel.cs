using System;

namespace Exoft.Gamification.Api.Common.Models
{
    public class ErrorResponseModel : ResponseModel
    {
        public ErrorResponseModel(string message) : base(false)
        {
            Message = message;
        }

        public ErrorResponseModel(Exception exception) : base(false)
        {
            Message = exception.ToString();
        }

        public string Message { get; set; }
    }
}
