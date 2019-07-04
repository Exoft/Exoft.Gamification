namespace Exoft.Gamification.Api.Data.Core.Helpers
{
    public class GamificationEnums
    {
        public enum EventType
        {
            Race = 0,
            Records = 1,
            Challenge = 2,
            Upload = 3
        }

        public enum ResponseType
        {
            Ok = 0,
            NotFound = 1,
            UnknownError = 2
        }
    }
}
