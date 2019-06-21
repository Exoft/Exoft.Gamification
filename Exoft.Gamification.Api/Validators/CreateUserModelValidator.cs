using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Data.Core.Interfaces;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Validators
{
    public class CreateUserModelValidator : AbstractValidator<CreateUserModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public CreateUserModelValidator
        (
            IUserRepository userRepository,
            IRoleRepository roleRepository
        )
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;

            RuleFor(user => user.FirstName)
                .NotEmpty().WithMessage(Resources.ErrorMessages.EmptyField)
                .MaximumLength(32).WithMessage(Resources.ErrorMessages.ToLong);

            RuleFor(user => user.LastName)
                .NotEmpty().WithMessage(Resources.ErrorMessages.EmptyField)
                .MaximumLength(32).WithMessage(Resources.ErrorMessages.ToLong);

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage(Resources.ErrorMessages.EmptyField)
                .MaximumLength(250).WithMessage(Resources.ErrorMessages.ToLong)
                .EmailAddress().WithMessage(Resources.ErrorMessages.WrongEmail)
                .MustAsync(CheckEmailAsync).WithMessage(Resources.ErrorMessages.UniqueEmail);

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage(Resources.ErrorMessages.EmptyField)
                .MinimumLength(8).WithMessage(Resources.ErrorMessages.ToShort)
                .MaximumLength(32).WithMessage(Resources.ErrorMessages.ToLong)
                .Matches("[A-Z]").WithMessage(Resources.ErrorMessages.PasswordUpperCaseLetter)
                .Matches("[a-z]").WithMessage(Resources.ErrorMessages.PasswordLowerCaseLetter)
                .Matches("[0-9]").WithMessage(Resources.ErrorMessages.PasswordDigit);

            RuleFor(user => user.Role)
                .NotEmpty().WithMessage(Resources.ErrorMessages.EmptyField)
                .MustAsync(CheckRoleAsync).WithMessage(Resources.ErrorMessages.WrongRole);

            RuleFor(user => user.UserName)
                .NotEmpty().WithMessage(Resources.ErrorMessages.EmptyField)
                .MaximumLength(32).WithMessage(Resources.ErrorMessages.ToLong)
                .MustAsync(CheckUserNameAsync).WithMessage(Resources.ErrorMessages.UniqueUserName);
        }

        private async Task<bool> CheckEmailAsync(string email, CancellationToken cancellationToken)
        {
            bool exists = await _userRepository.IsEmailExistsAsync(email);
            return !exists;
        }

        private async Task<bool> CheckRoleAsync(string role, CancellationToken cancellationToken)
        {
            var roleEntity = await _roleRepository.GetRoleByNameAsync(role);
            return roleEntity != null;
        }

        private async Task<bool> CheckUserNameAsync(string userName, CancellationToken cancellationToken)
        {
            var userEntity = await _userRepository.GetByUserNameAsync(userName);
            return userEntity == null;
        }
    }
}
