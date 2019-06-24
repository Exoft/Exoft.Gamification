using AutoMapper;
using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using File = Exoft.Gamification.Api.Data.Core.Entities.File;

namespace Exoft.Gamification.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserService
        (
            IUserRepository userRepository,
            IFileRepository fileRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork
        )
        {
            _userRepository = userRepository;
            _fileRepository = fileRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ReadFullUserModel> AddUserAsync(CreateUserModel model)
        {
            var user = _mapper.Map<User>(model);

            await _userRepository.AddAsync(user);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReadFullUserModel>(user);
        }

        public async Task DeleteUserAsync(Guid Id)
        {
            var user = await _userRepository.GetByIdAsync(Id);

            _userRepository.Delete(user);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ReturnPagingInfo<ReadShortUserModel>> GetAllUserAsync(PagingInfo pagingInfo)
        {
            var page = await _userRepository.GetAllDataAsync(pagingInfo);

            var readUserModel = page.Data.Select(i => _mapper.Map<ReadShortUserModel>(i)).ToList();
            var result = new ReturnPagingInfo<ReadShortUserModel>()
            {
                CurrentPage = page.CurrentPage,
                PageSize = page.PageSize,
                TotalItems = page.TotalItems,
                TotalPages = page.TotalPages,
                Data = readUserModel
            };

            return result;
        }

        public async Task<ReadFullUserModel> GetFullUserByIdAsync(Guid Id)
        {
            var user = await _userRepository.GetByIdAsync(Id);

            return _mapper.Map<ReadFullUserModel>(user);
        }

        public async Task<ReadShortUserModel> GetShortUserByIdAsync(Guid Id)
        {
            var user = await _userRepository.GetByIdAsync(Id);

            return _mapper.Map<ReadShortUserModel>(user);
        }

        public async Task<ReadFullUserModel> UpdateUserAsync(UpdateUserModel model, Guid Id)
        {
            var user = await _userRepository.GetByIdAsync(Id);
            user.UserName = model.UserName;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Status = model.Status;
            user.Email = model.Email;

            if (model.Avatar != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    await model.Avatar.CopyToAsync(memory);

                    if(user.AvatarId != null)
                    {
                        var file = await _fileRepository.GetByIdAsync(user.AvatarId.Value);
                        _fileRepository.Delete(file);
                    }
                    else
                    {
                        var file = new File()
                        {
                            Data = memory.ToArray(),
                            ContentType = model.Avatar.ContentType
                        };
                        await _fileRepository.AddAsync(file);
                        user.AvatarId = file.Id;
                    }
                }
            }
            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReadFullUserModel>(user);
        }
    }
}
