using System;
using System.Threading.Tasks;

using AutoMapper;

using Exoft.Gamification.Api.Common.Models.Thank;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces.Repositories;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;

namespace Exoft.Gamification.Api.Services
{
    public class ThankService : IThankService
    {
        private readonly IUserRepository _userRepository;
        private readonly IThankRepository _thankRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ThankService
        (
            IUserRepository userRepository,
            IThankRepository thankRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork
        )
        {
            _userRepository = userRepository;
            _thankRepository = thankRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(CreateThankModel model, Guid fromUserId)
        {
            var thank = _mapper.Map<Thank>(model);
            thank.FromUser = await _userRepository.GetByIdAsync(fromUserId);
            await _thankRepository.AddAsync(thank);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ReadThankModel> GetLastThankAsync(Guid toUserId)
        {
            var thankEntity = await _thankRepository.GetLastThankAsync(toUserId);

            return _mapper.Map<ReadThankModel>(thankEntity);
        }
    }
}
