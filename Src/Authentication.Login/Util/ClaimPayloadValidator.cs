using Authentication.Login.DTO;
using Authentication.Login.Resource;
using FluentValidation;
using System.Diagnostics;

namespace Authentication.Login.Util
{
    /// <summary>
    /// FluentValidation validator for ClaimPayLoadDTO objects.
    /// Defines validation rules for claim creation and update requests.
    /// Ensures claim type, value, and description meet business requirements.
    /// </summary>
    [DebuggerDisplay("ClaimPayloadValidator")]
    public class ClaimPayloadValidator : AbstractValidator<ClaimPayLoadDTO>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimPayloadValidator"/> class.
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

            // UpdatedBy validation rules (optional, but with length constraint)
            RuleFor(x => x.UpdatedBy)
                .MaximumLength(100).WithMessage(ResourceLogin.UpdatedByMustMost100Characters);
        }
    }
}
