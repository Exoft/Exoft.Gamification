using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

using AutoMapper;

using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services.Helpers;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Exoft.Gamification.Api.Services.Resources;

using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;

namespace Exoft.Gamification.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenProvider _refreshTokenProvider;
        private readonly IPasswordHasher _hasher;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly IJwtSecret _jwtSecret;
        private readonly IMapper _mapper;
        private readonly ICacheManager<Guid> _cache;
        private readonly IResetPasswordSettings _resetPasswordSettings;
        private readonly IUserAchievementRepository _userAchievementRepository;
        private readonly IStringLocalizer<HtmlPages> _stringLocalizer;

        public AuthService
        (
            IUserRepository userRepository,
            IRefreshTokenProvider authCacheManager,
            IPasswordHasher hasher,
            IEmailService emailService,
            IUserService userService,
            IJwtSecret jwtSecret,
            IMapper mapper,
            ICacheManager<Guid> cache,
            IResetPasswordSettings resetPasswordSettings,
            IUserAchievementRepository userAchievementRepository,
            IStringLocalizer<HtmlPages> stringLocalizer
        )
        {
            _userRepository = userRepository;
            _refreshTokenProvider = authCacheManager;
            _hasher = hasher;
            _emailService = emailService;
            _userService = userService;
            _jwtSecret = jwtSecret;
            _mapper = mapper;
            _cache = cache;
            _resetPasswordSettings = resetPasswordSettings;
            _userAchievementRepository = userAchievementRepository;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<JwtTokenModel> AuthenticateAsync(string login, string password)
        {
            var userEntity = await _userRepository.GetByUserNameAsync(login);

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

            return await GetJwtToken(userEntity, refreshToken);
        }

        public async Task<JwtTokenModel> AuthenticateByEmailAsync(string email, string password)
        {
            var userEntity = await _userRepository.GetByEmailAsync(email);

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

            return await GetJwtToken(userEntity, refreshToken);
        }

        public async Task<JwtTokenModel> RefreshTokenAsync(string refreshToken)
        {
            var refreshTokenFromDB = await _refreshTokenProvider.GetRefreshTokenInfo(refreshToken);
            if (refreshTokenFromDB == null)
            {
                return null;
            }

            var userId = refreshTokenFromDB.UserId;
            var userEntity = await _userRepository.GetByIdAsync(userId);
            if (userEntity == null)
            {
                return null;
            }

            return await GetJwtToken(userEntity, refreshTokenFromDB);
        }

        public async Task<IResponse> SendForgotPasswordAsync(string email, Uri resetPasswordPageLink)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return new NotFoundResponse("Email is not found!");
            }

            var temporaryRandomString = Guid.NewGuid().ToString();

            var cacheObject = new CacheObject<Guid>()
            {
                Key = temporaryRandomString,
                Value = user.Id,
                TimeToExpire = _resetPasswordSettings.TimeToExpireSecretString
            };
            await _cache.AddAsync(cacheObject);

            var uriBuilder = new UriBuilder(resetPasswordPageLink);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["secretString"] = temporaryRandomString;
            uriBuilder.Query = query.ToString();

            var forgotPasswordPage = _stringLocalizer["ForgotPasswordPage"].ToString();

            var pageWithParams = forgotPasswordPage.Replace("{link}", uriBuilder.ToString());

            await _emailService.SendEmailAsync("Reset your password", pageWithParams, email);

            return new OkResponse();
        }

        public async Task<IResponse> ResetPasswordAsync(string secretString, string newPassword)
        {
            var userId = await _cache.GetByKeyAsync(secretString);
            if (userId == default)
            {
                return new NotFoundResponse("Current secretString expired or not found!");
            }

            await _cache.DeleteAsync(secretString);

            await _userService.UpdatePasswordAsync(userId, newPassword);

            return new OkResponse();
        }

        private async Task<JwtTokenModel> GetJwtToken(User user, RefreshToken refreshToken)
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
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(_jwtSecret.Secret),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var jwtTokenModel = _mapper.Map<JwtTokenModel>(user);
            jwtTokenModel.BadgesCount = await _userAchievementRepository.GetCountAchievementsByUserAsync(user.Id);
            jwtTokenModel.Token = tokenHandler.WriteToken(token);
            jwtTokenModel.RefreshToken = refreshToken.Token;
            jwtTokenModel.TokenExpiration = token.ValidTo.ConvertToIso8601DateTimeUtc();

            return jwtTokenModel;
        }
    }
}
