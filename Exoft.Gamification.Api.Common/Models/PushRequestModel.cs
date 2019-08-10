using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exoft.Gamification.Api.Common.Models
{
    public class PushRequestModel
    {
        public OSType TargetOS { get; set; }
        public PushMessageModel PushMessage { get; set; }
    }

    public class PushMessageModel
    {
        [JsonProperty(PropertyName = "notification_content")]
        public Content NotificationContent { get; set; }

        public class Content
        {
            public string Name { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
        }
    }

    public enum OSType
    {
        ALL,
        ANDROID,
        IOS
    }
}
