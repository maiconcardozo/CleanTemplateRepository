using Authentication.Login.DTO;
using Authentication.Login.Resource;
using FluentValidation;

namespace Authentication.Login.Util
{
    /// <summary>
    /// FluentValidation validator for AccountPayLoadDTO objects.
    /// Defines validation rules for user account creation and authentication requests.
    /// Ensures username and password meet security and business requirements.
    /// </summary>
    public class AccountPayloadValidator : AbstractValidator<AccountPayLoadDTO>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountPayloadValidator"/> class.
        /// Initializes validation rules for account payload.
        /// Validates username and password format, length, and content restrictions.
        /// </summary>
        public AccountPayloadValidator()
        {
            // Username validation rules
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage(ResourceLogin.UserNameRequired)
                .Must(u => !string.IsNullOrWhiteSpace(u) && !u.Contains(" ", StringComparison.Ordinal)).WithMessage(ResourceLogin.UserNameCannotContainSpacesNullEmpty)
                .MinimumLength(6).WithMessage(ResourceLogin.UserNameMustLeast6Characters)
                .MaximumLength(50).WithMessage(ResourceLogin.UserNameMustMost50Characters);

            // Password validation rules
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(ResourceLogin.PasswordRequired)
                .Must(p => !string.IsNullOrWhiteSpace(p) && !p.Contains(" ", StringComparison.Ordinal)).WithMessage(ResourceLogin.PasswordCannotContainSpacesNullEmpty)
                .MinimumLength(6).WithMessage(ResourceLogin.PasswordMustLeast6Characters)
                .MaximumLength(50).WithMessage(ResourceLogin.PasswordMustMost50Characters);

            // UpdatedBy validation rules (optional, but with length constraint)
            RuleFor(x => x.UpdatedBy)
                .MaximumLength(100).WithMessage(ResourceLogin.UpdatedByMustMost100Characters);
        }
    }
}
