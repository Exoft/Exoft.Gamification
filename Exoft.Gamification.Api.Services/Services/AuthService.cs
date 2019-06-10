using AutoMapper;
using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Repositories;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Exoft.Gamification.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserRepository userRepository;
        private readonly IJwtSecret jwtSecret;
        private readonly IMapper mapper;
        
        public AuthService(UsersDbContext context, IJwtSecret jwtSecret, IMapper mapper)
        {
            this.userRepository = new UserRepository(context);
            this.jwtSecret = jwtSecret;
            this.mapper = mapper;
        }

        public JwtTokenModel Authenticate(string userName, string password)
        {
            var userEntity = userRepository.GetUserAsync(userName).Result;

            if (userEntity == null || password != userEntity.Password)
            {
                return null;
            }

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();

            // TODO fix
            var key = Encoding.ASCII.GetBytes("THIS IS USED TO SIGN AND VERIFY JWT TOKENS, REPLACE IT WITH YOUR OWN SECRET, IT CAN BE ANY STRING");
            //var key = Encoding.ASCII.GetBytes(_jwtSecret.TokenSecretString);
            //

            var claims = new List<Claim>();
            foreach (var item in userEntity.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item.Role.Name));
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var jwtTokenModel = new JwtTokenModel()
            {
                Token = tokenHandler.WriteToken(token)
            };

            return jwtTokenModel;
        }
    }
}
