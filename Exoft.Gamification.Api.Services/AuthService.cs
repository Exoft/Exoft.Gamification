using AutoMapper;
using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
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
        private readonly IAuthCacheManager _authCacheManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtSecret _jwtSecret;
        private readonly IPasswordHasher _hasher;
        private readonly IMapper _mapper;

        public AuthService
        (
            IUserRepository userRepository,
            IAuthCacheManager authCacheManager,
            IUnitOfWork unitOfWork,
            IJwtSecret jwtSecret,
            IPasswordHasher hasher,
            IMapper mapper
        )
        {
            _userRepository = userRepository;
            _authCacheManager = authCacheManager;
            _unitOfWork = unitOfWork;
            _jwtSecret = jwtSecret;
            _hasher = hasher;
            _mapper = mapper;
        }

        public async Task<JwtTokenModel> AuthenticateAsync(string userName, string password)
        {
            var userEntity = await _userRepository.GetByUserNameAsync(userName);

            if (userEntity == null || !_hasher.Equals(password, userEntity.Password))
            {
                return null;
            }
            
            var newRefreshToken = new RefreshToken()
            {
                Token = Guid.NewGuid().ToString(),
                UserId = userEntity.Id
            };
            await _authCacheManager.AddAsync(newRefreshToken);

            return GetJwtToken(userEntity, newRefreshToken);
        }

        public async Task<JwtTokenModel> RefreshTokenAsync(string refreshToken)
        {
            var refreshTokenFromDB = await _authCacheManager.GetByKeyAsync(refreshToken);
            if(refreshTokenFromDB == null)
            {
                return null;
            }

            var userEntity = await _userRepository.GetByIdAsync(refreshTokenFromDB.UserId);
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
                Expires = DateTime.UtcNow.Add(_jwtSecret.SecondsToExpireToken),
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
