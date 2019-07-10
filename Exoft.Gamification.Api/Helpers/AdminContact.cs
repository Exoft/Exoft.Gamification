using Exoft.Gamification.Api.Common.Helpers;
using Microsoft.Extensions.Configuration;

namespace Exoft.Gamification.Api.Helpers
{
    public class AdminContact : IAdminContact
    {
        public AdminContact(IConfiguration configuration)
        {
            var section = configuration.GetSection("AdminContact");

            Mail = section.GetValue<string>("Mail");
        }

        public string Mail { get; set; }
    }
}
