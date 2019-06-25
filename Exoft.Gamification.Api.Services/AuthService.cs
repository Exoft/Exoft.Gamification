using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtSecret _jwtSecret;
        private readonly IMD5Hash _MD5Hash;

        public AuthService
        (
            IUserRepository userRepository,
            IAuthRepository authRepository,
            IUnitOfWork unitOfWork,
            IJwtSecret jwtSecret,
            IMD5Hash MD5Hash
        )
        {
            _userRepository = userRepository;
            _authRepository = authRepository;
            _unitOfWork = unitOfWork;
            _jwtSecret = jwtSecret;
            _MD5Hash = MD5Hash;
        }

        public async Task<JwtTokenModel> Authenticate(string userName, string password)
        {
            var userEntity = await _userRepository.GetByUserNameAsync(userName);

            // refresh token
            var refreshTokenFromDB = _authRepository.GetByUserId(userEntity.Id);
            if (refreshTokenFromDB != null)
            {
                _authRepository.Delete(refreshTokenFromDB);
            }

            var newRefreshToken = new RefreshToken()
            {
                ExpiresUtc = DateTime.Now.AddSeconds(_jwtSecret.SecondsToExpireRefreshToken),
                Token = Guid.NewGuid().ToString(),
                UserId = userEntity.Id
            };
            _authRepository.Add(newRefreshToken);
            
            if (userEntity == null || _MD5Hash.GetMD5Hash(password) != userEntity.Password.ToLower())
            {
                return null;
            }

            return GetJwtToken(userEntity, newRefreshToken);
        }

        public async Task<JwtTokenModel> RefreshTokenAsync(RefreshTokenModel model)
        {
            var refreshTokenFromDB = _authRepository.GetByUserId(model.UserId);
            var userEntity = await _userRepository.GetByIdAsync(model.UserId);
            if(refreshTokenFromDB == null || userEntity == null)
            {
                return null;
            }
            if(refreshTokenFromDB.ExpiresUtc < DateTime.UtcNow)
            {
                return null;
            }

            return GetJwtToken(userEntity, refreshTokenFromDB);
        }

        private JwtTokenModel GetJwtToken(User user, RefreshToken refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            foreach (var item in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item.Role.Name));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(_jwtSecret.SecondsToExpireToken),
                SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(_jwtSecret.Secret),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new JwtTokenModel
            {
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token,
                TokenExpiration = token.ValidTo.ConvertToIso8601DateTimeUtc()
            };
        }
    }
}
