using AutoMapper;
using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Interfaces;
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
        private readonly IUserService _userService;
        private readonly IJwtSecret _jwtSecret;
        private readonly IMapper mapper;
        
        public AuthService(IUserService userService, IJwtSecret jwtSecret, IMapper mapper)
        {
            _userService = userService;
            _jwtSecret = jwtSecret;
            this.mapper = mapper;
        }

        public UserModel Authenticate(string userName, string password)
        {
            var userEntity = _userService.GetUserAsync(userName).Result;
            
            if(userEntity==null || password != userEntity.Password)
            {
                return null;
            }
            
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();

            // TODO fix
            var key = Encoding.ASCII.GetBytes("THIS IS USED TO SIGN AND VERIFY JWT TOKENS, REPLACE IT WITH YOUR OWN SECRET, IT CAN BE ANY STRING");
            //var key = Encoding.ASCII.GetBytes(_jwtSecret.TokenSecretString);
            //

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userEntity.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var userModel = mapper.Map<User, UserModel>(userEntity);

            userModel.Token = tokenHandler.WriteToken(token);

            return userModel;
        }
    }
}
