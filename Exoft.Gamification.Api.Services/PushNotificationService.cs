using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services
{
    public class PushNotificationService : IPushNotificationService
    {
        private readonly IAppCenterSettings _appCenterSettings;
        private readonly HttpClient client;
        private const string mediaType = "application/json";
        public PushNotificationService(IAppCenterSettings appCenterSettings)
        {
            _appCenterSettings = appCenterSettings;
            client = new HttpClient();
            client.DefaultRequestHeaders.Add(_appCenterSettings.KeyNameToken, _appCenterSettings.Token);
        }
        public async Task<(OSType type,bool isSend)> SendNotification(PushRequestModel pushRequestModel)
        {
            bool responseIOS, responseAndroid;
            switch (pushRequestModel.TargetOS)
            {
                case OSType.ALL:
                    responseIOS = await SendToIOS(pushRequestModel.PushMessage);
                    responseAndroid = await SendToAndroid(pushRequestModel.PushMessage);
                    return (OSType.ALL, responseAndroid && responseIOS);
                case OSType.ANDROID:
                    responseAndroid = await SendToAndroid(pushRequestModel.PushMessage);
                    return (OSType.ANDROID, responseAndroid);
                case OSType.IOS:
                    responseIOS = await SendToIOS(pushRequestModel.PushMessage);
                    return (OSType.IOS, responseIOS);
                default: return (OSType.ALL, false);
            }
        }

        protected virtual async Task<bool> SendToIOS(PushMessageModel pushMessageModel) {
            var jsonModel = JsonConvert.SerializeObject(pushMessageModel);
            HttpContent jsonContent = new StringContent(jsonModel, Encoding.UTF8, mediaType);
            var response = await client.PostAsync(_appCenterSettings.UrlForPushIOS, jsonContent);
            return response.IsSuccessStatusCode;
        }

        protected virtual async Task<bool> SendToAndroid(PushMessageModel pushMessageModel)
        {
            var jsonModel = JsonConvert.SerializeObject(pushMessageModel);
            HttpContent jsonContent = new StringContent(jsonModel, Encoding.UTF8, mediaType);
            var response = await client.PostAsync(_appCenterSettings.UrlForPushAndroid, jsonContent);
            return response.IsSuccessStatusCode;
        }
    }
}
