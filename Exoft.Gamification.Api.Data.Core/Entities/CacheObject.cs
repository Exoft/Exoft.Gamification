using System;

namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class CacheObject
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public TimeSpan TimeToExpire { get; set; }
    }
}
