using System;
using System.Collections.Generic;
using System.Text;

namespace Exoft.Gamification.Api.Common.Models
{
    public class ResponseModelWithData<T> : ResponseModel where T: class
    {
        public ResponseModelWithData(T data) : base(true)
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}
