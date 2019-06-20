using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
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

        public AuthService
        (
            IUserRepository userRepository,
            IAuthRepository authRepository,
            IUnitOfWork unitOfWork,
            IJwtSecret jwtSecret
        )
        {
            _userRepository = userRepository;
            _authRepository = authRepository;
            _unitOfWork = unitOfWork;
            _jwtSecret = jwtSecret;
        }

        public async Task<JwtTokenModel> Authenticate(string userName, string password)
        {
            var userEntity = await _userRepository.GetByUserNameAsync(userName);

            // refresh token
            var refreshTokenFromDB = await _authRepository.GetByUserIdAsync(userEntity.Id);
            if (refreshTokenFromDB != null)
            {
                _authRepository.Delete(refreshTokenFromDB);
                await _unitOfWork.SaveChangesAsync();
            }

            var newRefreshToken = new RefreshToken()
            {
                ExpiresUtc = DateTime.Now.AddHours(Convert.ToDouble(_jwtSecret.ExpireRefreshToken)),
                Token = Guid.NewGuid().ToString(),
                User = userEntity
            };
            await _authRepository.AddAsync(newRefreshToken);
            await _unitOfWork.SaveChangesAsync();
            
            if (userEntity == null || password.GetMD5Hash() != userEntity.Password)
            {
                return null;
            }

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userEntity.Id.ToString())
            };

            foreach (var item in userEntity.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item.Role.Name));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(_jwtSecret.ExpireToken)),
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
                RefreshToken = newRefreshToken.Token,
                TokenExpiration = token.ValidTo
            };
        }

        public async Task<JwtTokenModel> RefreshTokenAsync(RefreshTokenModel model)
        {
            var refreshTokenFromDB = await _authRepository.GetByUserIdAsync(model.UserId);
            var userEntity = await _userRepository.GetByIdAsync(model.UserId);
            if(refreshTokenFromDB == null || userEntity == null)
            {
                return null;
            }
            if(refreshTokenFromDB.ExpiresUtc < DateTime.Now.ToUniversalTime())
            {
                return null;
            }

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userEntity.Id.ToString())
            };

            foreach (var item in userEntity.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item.Role.Name));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(_jwtSecret.ExpireToken)),
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
                RefreshToken = refreshTokenFromDB.Token,
                TokenExpiration = token.ValidTo
            };
        }
    }
}
