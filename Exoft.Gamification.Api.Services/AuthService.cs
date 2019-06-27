using AutoMapper;
using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly IJwtSecret _jwtSecret;
        private readonly IMapper _mapper;

        public AuthService
        (
            IUserRepository userRepository,
            IEmailService emailService,
            IUserService userService,
            IJwtSecret jwtSecret,
            IMapper mapper
        )
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _userService = userService;
            _jwtSecret = jwtSecret;
            _mapper = mapper;
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

            var jwtTokenModel = _mapper.Map<JwtTokenModel>(userEntity);
            jwtTokenModel.Token = tokenHandler.WriteToken(token);

            return jwtTokenModel;
        }

        public async Task ResetPasswordAsync(string email)
        {
            var userEntity = await _userRepository.GetByEmailAsync(email);
            if(userEntity == null)
            {
                throw new ArgumentNullException();
            }

            var newPassword = await _userService.GenerateNewPasswordAsync(userEntity.Id);

            StringBuilder builder = new StringBuilder();
            builder.Append("Your new password: ");
            builder.Append(newPassword);

            await _emailService.SendEmailAsync(email, "Reset your password", builder.ToString());
        }
    }
}
