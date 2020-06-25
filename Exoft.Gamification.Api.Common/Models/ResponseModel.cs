namespace Exoft.Gamification.Api.Common.Models
{
    public abstract class ResponseModel
    {
        protected ResponseModel(bool isSussessful)
        {
            IsSuccessful = isSussessful;
        }

        public bool IsSuccessful { get; }
    }
}
