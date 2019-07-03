using System;

namespace Exoft.Gamification.Api.Common.Models.Thank
{
    public class CreateThankModel
    {
        public string Text { get; set; }

        public Guid ToUserId { get; set; }
    }
}
