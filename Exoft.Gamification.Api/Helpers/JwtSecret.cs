using Exoft.Gamification.Api.Common.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Helpers
{
    public class JwtSecret : IJwtSecret
    {
        //public JwtSecret(IConfiguration configuration, IServiceCollection services)
        //{
        //    Configuration = configuration;
        //    Services = services;
        //}

        //private IConfiguration Configuration { get; set; }

        //private IServiceCollection Services { get; set; }

        public string TokenSecretString
        {
            //get
            //{
            //    var secretSection = Configuration.GetSection("Secrets");
            //    Services.Configure<JwtSecret>(secretSection);
            //    var jwtSecret = secretSection.Get<JwtSecret>();
            //    return jwtSecret.TokenSecretString;
            //}
            get; set;
        }
    }
}
