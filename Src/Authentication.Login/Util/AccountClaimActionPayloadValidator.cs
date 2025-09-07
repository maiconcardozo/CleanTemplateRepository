using Authentication.Login.DTO;
using Authentication.Login.Resource;
using FluentValidation;

namespace Authentication.Login.Util
{
    /// <summary>
    /// FluentValidation validator for AccountClaimActionPayLoadDTO objects.
    /// Defines validation rules for account claim action association requests.
    /// Ensures account and claim action identifiers are valid positive integers.
    /// </summary>
    public class AccountClaimActionPayloadValidator : AbstractValidator<AccountClaimActionPayLoadDTO>
    {
        /// <summary>
        /// Initializes validation rules for account claim action payload.
        /// Validates account ID and claim action ID are positive integers.
        /// </summary>
        public AccountClaimActionPayloadValidator()
        {
            // IdAccount validation rules
            RuleFor(x => x.IdAccount)
                .NotEmpty().WithMessage(ResourceLogin.IdAccountRequired)
                .GreaterThan(0).WithMessage(ResourceLogin.IdAccountMustBePositive);

            // IdClaimAction validation rules
            RuleFor(x => x.IdClaimAction)
                .NotEmpty().WithMessage(ResourceLogin.IdClaimActionRequired)
                .GreaterThan(0).WithMessage(ResourceLogin.IdClaimActionMustBePositive);

            // CreatedBy validation rules (required for audit tracking)
            RuleFor(x => x.CreatedBy)
                .NotEmpty().WithMessage(ResourceLogin.CreatedByRequired)
                .MaximumLength(100).WithMessage(ResourceLogin.CreatedByMustMost100Characters);

            // UpdatedBy validation rules (optional for create, but with length constraint)
            RuleFor(x => x.UpdatedBy)
                .MaximumLength(100).WithMessage(ResourceLogin.UpdatedByMustMost100Characters);
        }
    }
}