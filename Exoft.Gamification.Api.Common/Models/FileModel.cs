using System;

namespace Exoft.Gamification.Api.Common.Models
{
    public class FileModel
    {
        public Guid Id { get; set; }

        public string ContentType { get; set; }

        public byte[] Data { get; set; }
    }
}
