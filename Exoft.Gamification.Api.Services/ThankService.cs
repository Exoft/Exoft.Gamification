using AutoMapper;
using Exoft.Gamification.Api.Common.Models.Thank;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Services
{
    public class ThankService : IThankService
    {
        private readonly IThankRepository _thankRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ThankService
        (
            IThankRepository thankRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork
        )
        {
            _thankRepository = thankRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(CreateThankModel model, Guid fromUserId)
        {
            var thankEntity = await _thankRepository.GetThankAsync(model.ToUserId);
            if(thankEntity != null)
            {
                _thankRepository.Delete(thankEntity);
            }

            var thank = _mapper.Map<Thank>(model);
            thank.FromUserId = fromUserId;
            await _thankRepository.AddAsync(thank);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ReadThankModel> GetThankAsync(Guid toUserId)
        {
            var thankEntity = await _thankRepository.GetThankAsync(toUserId);
            if(thankEntity == null)
            {
                return null;
            }

            return _mapper.Map<ReadThankModel>(thankEntity);
        }
    }
}
