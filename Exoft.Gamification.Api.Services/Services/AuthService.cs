using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models;
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
        private readonly JwtSecret _jwtSecret;
        
        public AuthService(IUserService userService, IOptions<JwtSecret> jwtSecret)
        {
            _userService = userService;
            _jwtSecret = jwtSecret.Value;
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
            var key = Encoding.ASCII.GetBytes(_jwtSecret.TokenSecretString);
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
            UserModel userModel = new UserModel()
            {
                Id = userEntity.Id,
                UserName = userEntity.UserName,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                Email = userEntity.Email,
                Status = userEntity.Status,
                Avatar = userEntity.Avatar,
                XP = userEntity.XP,
                //Achievements = user.Achievements,
                Token = tokenHandler.WriteToken(token)
            };

            return userModel;
        }
    }
}
