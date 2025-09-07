using Authentication.Login.DTO;
using Authentication.Login.Resource;
using FluentValidation;

namespace Authentication.Login.Util
{
    /// <summary>
    /// FluentValidation validator for ClaimActionPayLoadDTO objects.
    /// Defines validation rules for claim action association requests.
    /// Ensures claim and action identifiers are valid positive integers.
    /// </summary>
    public class ClaimActionPayloadValidator : AbstractValidator<ClaimActionPayLoadDTO>
    {
        /// <summary>
        /// Initializes validation rules for claim action payload.
        /// Validates claim ID and action ID are positive integers.
        /// </summary>
        public ClaimActionPayloadValidator()
        {
            // IdClaim validation rules
            RuleFor(x => x.IdClaim)
                .NotEmpty().WithMessage(ResourceLogin.IdClaimRequired)
                .GreaterThan(0).WithMessage(ResourceLogin.IdClaimMustBePositive);

            // IdAction validation rules
            RuleFor(x => x.IdAction)
                .NotEmpty().WithMessage(ResourceLogin.IdActionRequired)
                .GreaterThan(0).WithMessage(ResourceLogin.IdActionMustBePositive);

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