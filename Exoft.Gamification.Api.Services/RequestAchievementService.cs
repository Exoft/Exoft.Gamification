using AutoMapper;
using Exoft.Gamification.Api.Common.Helpers;
using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services.Helpers;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Exoft.Gamification.Api.Services.Resources;
using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services
{
    public class RequestAchievementService : IRequestAchievementService
    {
        private readonly IRequestAchievementRepository _requestAchievementRepository;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IAchievementRepository _achievementRepository;
        private readonly IStringLocalizer<HtmlPages> _stringLocalizer;
        private readonly IEmailSenderSettings _emailSenderSettings;
        private readonly IUnitOfWork _unitOfWork;

        public RequestAchievementService
        (
            IRequestAchievementRepository requestAchievementRepository,
            IEmailService emailService,
            IMapper mapper,
            IUserRepository userRepository,
            IAchievementRepository achievementRepository,
            IStringLocalizer<HtmlPages> stringLocalizer,
            IEmailSenderSettings emailSenderSettings,
            IUnitOfWork unitOfWork
        )
        {
            _requestAchievementRepository = requestAchievementRepository;
            _emailService = emailService;
            _mapper = mapper;
            _userRepository = userRepository;
            _achievementRepository = achievementRepository;
            _stringLocalizer = stringLocalizer;
            _emailSenderSettings = emailSenderSettings;
            _unitOfWork = unitOfWork;
        }

        public async Task<IResponse> AddAsync(RequestAchievementModel model, Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            var achievement = await _achievementRepository.GetByIdAsync(model.AchievementId);

            var requestAchievementPage = _stringLocalizer["RequestAchievementPage"].ToString();
            var pageWithParams = requestAchievementPage.Replace("{userLastName}", user.LastName);
            pageWithParams = pageWithParams.Replace("{userFirstName}", user.FirstName);
            pageWithParams = pageWithParams.Replace("{message}", model.Message);
            pageWithParams = pageWithParams.Replace("{achievementName}", achievement.Name);

            await _emailService.SendEmailAsync(
                _emailSenderSettings.Email,
                "Request achievement",
                pageWithParams);
            
            var entity = _mapper.Map<RequestAchievement>(model);
            entity.UserId = userId;
            await _requestAchievementRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return new OkResponse();
        }
    }
}
