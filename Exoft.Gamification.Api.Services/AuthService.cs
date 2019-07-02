using AutoMapper;
using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
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
        private readonly IRefreshTokenProvider _refreshTokenProvider;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtSecret _jwtSecret;
        private readonly IMapper _mapper;

        public AuthService
        (
            IUserRepository userRepository,
            IRefreshTokenProvider authCacheManager,
            IPasswordHasher hasher,
            IJwtSecret jwtSecret,
            IMapper mapper
        )
        {
            _userRepository = userRepository;
            _refreshTokenProvider = authCacheManager;
            _hasher = hasher;
            _jwtSecret = jwtSecret;
            _mapper = mapper;
        }

        public async Task<JwtTokenModel> AuthenticateAsync(string userName, string password)
        {
            var userEntity = await _userRepository.GetByUserNameAsync(userName);

            if (userEntity == null || !_hasher.Compare(password, userEntity.Password))
            {
                return null;
            }
            
            var refreshToken = new RefreshToken()
            {
                Token = Guid.NewGuid().ToString(),
                UserId = userEntity.Id
            };
            await _refreshTokenProvider.AddAsync(refreshToken);

            return GetJwtToken(userEntity, refreshToken);
        }

        public async Task<JwtTokenModel> RefreshTokenAsync(string refreshToken)
        {
            var refreshTokenFromDB = await _refreshTokenProvider.GetRefreshTokenInfo(refreshToken);
            if(refreshTokenFromDB == null)
            {
                return null;
            }
            
            var userId = refreshTokenFromDB.UserId;
            var userEntity = await _userRepository.GetByIdAsync(userId);
            if(userEntity == null)
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
                Expires = DateTime.UtcNow.Add(_jwtSecret.TimeToExpireToken),
                SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(_jwtSecret.Secret),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var jwtTokenModel = _mapper.Map<JwtTokenModel>(user);
            jwtTokenModel.Token = tokenHandler.WriteToken(token);
            jwtTokenModel.RefreshToken = refreshToken.Token;
            jwtTokenModel.TokenExpiration = token.ValidTo.ConvertToIso8601DateTimeUtc();

            return jwtTokenModel;
        }
    }
}
