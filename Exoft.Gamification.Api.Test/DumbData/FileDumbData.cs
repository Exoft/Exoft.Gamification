using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Test.TestData;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

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

        public static async Task<File> GetEntity(IFormFile file)
        {
            using var memory = new System.IO.MemoryStream();
            await file.CopyToAsync(memory);

            var result = new File
            {
                ContentType = file.ContentType,
                Data = memory.ToArray()
            };
            return result;
        }

        public static FormFile GetFile()
        {
            var myIcon = Resources.Resource.star_small_image;
            var stream = new System.IO.MemoryStream(myIcon);
            var file = new FormFile(stream, 0, stream.Length, null, "Star")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/png"
            };
            return file;
        }
    }
}
