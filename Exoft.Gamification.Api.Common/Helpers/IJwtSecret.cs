using System;
using System.Collections.Generic;
using System.Text;

namespace Exoft.Gamification.Api.Common.Helpers
{
    public interface IJwtSecret
    {
        string TokenSecretString { get; }
    }
}
