using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Test.TestData;
using System;

namespace Exoft.Gamification.Api.Test.DumbData
{
    public static class FileDumbData
    {
        public static File GetEntity()
        {
            var rnd = new Random();
            var result = new File
            {
                ContentType = RandomHelper.GetRandomString(),
                Data = new byte[RandomHelper.GetRandomNumber()]
            };
            rnd.NextBytes(result.Data);
            return result;
        }
    }
}
