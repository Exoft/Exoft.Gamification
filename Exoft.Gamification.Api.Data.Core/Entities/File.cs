namespace Exoft.Gamification.Api.Data.Core.Entities
{
    public class File : Entity
    {
        public string ContentType { get; set; }

        public byte[] Data { get; set; }
    }
}
