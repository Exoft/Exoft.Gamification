using Exoft.Gamification.Api.Data.Core.Entities;
using System;

namespace Exoft.Gamification.Api.Data.Core.Interfaces
{
    public interface IAuthRepository
    {
        void Add(RefreshToken entity);
        void Delete(RefreshToken entity);
        RefreshToken GetByUserId(Guid userId);
    }
}
