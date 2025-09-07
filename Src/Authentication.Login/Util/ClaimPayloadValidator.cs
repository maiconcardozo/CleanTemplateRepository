using Authentication.Login.DTO;
using Authentication.Login.Resource;
using FluentValidation;

namespace Authentication.Login.Util
{
    /// <summary>
    /// FluentValidation validator for ClaimPayLoadDTO objects.
    /// Defines validation rules for claim creation and update requests.
    /// Ensures claim type, value, and description meet business requirements.
    /// </summary>
    public class ClaimPayloadValidator : AbstractValidator<ClaimPayLoadDTO>
    {
        /// <summary>
        /// Initializes validation rules for claim payload.
        /// Validates claim type, value format and length, and description constraints.
        /// </summary>
        public ClaimPayloadValidator()
        {
            // Type validation rules (enum validation)
            RuleFor(x => x.Type)
                .IsInEnum().WithMessage(ResourceLogin.ClaimTypeRequired);

            // Value validation rules
            RuleFor(x => x.Value)
                .NotEmpty().WithMessage(ResourceLogin.ClaimValueRequired)
                .MinimumLength(1).WithMessage(ResourceLogin.ClaimValueMustLeast1Character)
                .MaximumLength(200).WithMessage(ResourceLogin.ClaimValueMustMost200Characters);

            // Description validation rules (optional field, but with length constraint)
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage(ResourceLogin.ClaimDescriptionMustMost500Characters);

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