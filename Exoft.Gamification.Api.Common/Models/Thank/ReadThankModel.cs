using System;

namespace Exoft.Gamification.Api.Common.Models.Thank
{
    public class ReadThankModel
    {
        public Guid FromUserId { get; set; }

        public string Text { get; set; }
    }
}
