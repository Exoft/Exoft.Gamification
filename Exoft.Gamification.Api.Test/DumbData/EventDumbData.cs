using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Test.TestData;
using System;
using System.Collections.Generic;
using static Exoft.Gamification.Api.Data.Core.Helpers.GamificationEnums;

namespace Exoft.Gamification.Api.Test.DumbData
{
    public class EventDumbData
    {
        public static Event GetRandomEntity()
        {
            var rnd = new Random();
            return new Event
            {
                Description = RandomHelper.GetRandomString(200),
                User = UserDumbData.GetRandomEntity(),
                Type = (EventType)rnd.Next(Enum.GetNames(typeof(EventType)).Length)
            };
        }

        public static List<Event> GetRandomEntities(int number)
        {
            var list = new List<Event>();
            for (int i = 0; i < number; i++)
            {
                list.Add(GetRandomEntity());
            }
            return list;
        }
    }
}
