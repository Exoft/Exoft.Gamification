using System;

namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class CacheObject<T>
    {
        public string Key { get; set; }

        public T Value { get; set; }

        public TimeSpan TimeToExpire { get; set; }
    }
}
