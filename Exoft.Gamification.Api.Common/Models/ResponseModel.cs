namespace Exoft.Gamification.Api.Common.Models
{
    public abstract class ResponseModel
    {
        public ResponseModel(bool isSussessful)
        {
            IsSuccessful = isSussessful;
        }

        public bool IsSuccessful { get; }
    }
}
