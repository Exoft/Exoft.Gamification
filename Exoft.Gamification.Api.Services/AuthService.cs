using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Interfaces;
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
        private readonly IJwtSecret _jwtSecret;

        public AuthService
        (
            IUserRepository userRepository,
            IJwtSecret jwtSecret
        )
        {
            _userRepository = userRepository;
            _jwtSecret = jwtSecret;
        }

        public async Task<JwtTokenModel> Authenticate(string userName, string password)
        {
            var userEntity = await _userRepository.GetByUserNameAsync(userName);

            //TODO: provide password hash
            if (userEntity == null || password != userEntity.Password)
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
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(_jwtSecret.Secret),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);


            return new JwtTokenModel
            {
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}
