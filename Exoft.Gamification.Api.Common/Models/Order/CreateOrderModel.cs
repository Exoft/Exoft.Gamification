using System;
using System.Collections.Generic;

using Exoft.Gamification.Api.Common.Helpers;

using Microsoft.AspNetCore.Http;

namespace Exoft.Gamification.Api.Common.Models.Order
{
    public class CreateOrderModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public IEnumerable<Guid> CategoryIds { get; set; }

        [NonLogged]
        public IFormFile Icon { get; set; }
    }
}