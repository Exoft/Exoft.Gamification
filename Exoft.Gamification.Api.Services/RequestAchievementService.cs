using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Exoft.Gamification.Api.Common.Models.RequestAchievement;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services.Helpers;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using Exoft.Gamification.Api.Services.Resources;

using Microsoft.Extensions.Localization;

namespace Exoft.Gamification.Api.Services
{
    public class RequestAchievementService : IRequestAchievementService
    {
        private readonly IRequestAchievementRepository _requestAchievementRepository;
        private readonly IUserAchievementService _userAchievementService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IAchievementRepository _achievementRepository;
        private readonly IStringLocalizer<HtmlPages> _stringLocalizer;
        private readonly IUnitOfWork _unitOfWork;

        public RequestAchievementService
        (
            IRequestAchievementRepository requestAchievementRepository,
            IEmailService emailService,
            IMapper mapper,
            IUserAchievementService userAchievementService,
            IUserRepository userRepository,
            IAchievementRepository achievementRepository,
            IStringLocalizer<HtmlPages> stringLocalizer,
            IUnitOfWork unitOfWork
        )
        {
            _requestAchievementRepository = requestAchievementRepository;
            _emailService = emailService;
            _mapper = mapper;
            _userRepository = userRepository;
            _achievementRepository = achievementRepository;
            _stringLocalizer = stringLocalizer;
            _unitOfWork = unitOfWork;
            _userAchievementService = userAchievementService;
        }

        public async Task<IResponse> AddAsync(CreateRequestAchievementModel model, Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            var achievement = await _achievementRepository.GetByIdAsync(model.AchievementId);

            var requestAchievementPage = _stringLocalizer["RequestAchievementPage"].ToString();
            var pageWithParams = requestAchievementPage.Replace("{userLastName}", user.LastName);
            pageWithParams = pageWithParams.Replace("{userFirstName}", user.FirstName);
            pageWithParams = pageWithParams.Replace("{message}", model.Message);
            pageWithParams = pageWithParams.Replace("{achievementName}", achievement.Name);

            var emails = await _userRepository.GetAdminsEmailsAsync();

            await _emailService.SendEmailsAsync("Request achievement", pageWithParams, emails.ToArray());

            var entity = _mapper.Map<RequestAchievement>(model);
            entity.UserId = userId;
            await _requestAchievementRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return new OkResponse();
        }

        public async Task DeleteAsync(RequestAchievement achievementRequest)
        {
            _requestAchievementRepository.Delete(achievementRequest);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<ReadRequestAchievementModel>> GetAllAsync()
        {
            var requestAchievementModels = new List<ReadRequestAchievementModel>();
            var requestAchievements = (await _requestAchievementRepository.GetAllDataAsync(new PagingInfo())).Data.ToList();
            foreach (var item in requestAchievements)
            {
                var user = await _userRepository.GetByIdAsync(item.UserId);
                var achievement = await _achievementRepository.GetByIdAsync(item.AchievementId);
                requestAchievementModels.Add(new ReadRequestAchievementModel
                {
                    Id = item.Id,
                    AchievementId = item.AchievementId,
                    UserId = item.UserId,
                    AchievementName = achievement.Name,
                    UserName = user.UserName,
                    Message = item.Message
                });
            }

            return requestAchievementModels;
        }

        public async Task<RequestAchievement> GetByIdAsync(Guid id)
        {
            return await _requestAchievementRepository.GetByIdAsync(id);
        }

        public async Task ApproveAchievementRequestAsync(Guid id)
        {
            var achievementRequest = await _requestAchievementRepository.GetByIdAsync(id);
            await _userAchievementService.AddAsync(achievementRequest.UserId, achievementRequest.AchievementId);
            await DeleteAsync(achievementRequest);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
