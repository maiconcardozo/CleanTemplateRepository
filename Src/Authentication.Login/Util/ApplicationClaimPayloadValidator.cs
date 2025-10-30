using Authentication.Login.DTO;
using FluentValidation;

namespace Authentication.Login.Util
{
    /// <summary>
    /// FluentValidation validator for ApplicationClaimPayLoadDTO objects.
    /// Defines validation rules for application-claim mapping creation and update requests.
    /// Ensures application and claim references are valid.
    /// </summary>
    public class ApplicationClaimPayloadValidator : AbstractValidator<ApplicationClaimPayLoadDTO>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationClaimPayloadValidator"/> class.
        /// Initializes validation rules for application-claim mapping payload.
        /// Validates that application and claim IDs are properly set.
        /// </summary>
        public ApplicationClaimPayloadValidator()
        {
            // IdApplication validation rules
            RuleFor(x => x.IdApplication)
                .GreaterThan(0).WithMessage("Application ID must be greater than 0.");

            // IdClaim validation rules
            RuleFor(x => x.IdClaim)
                .GreaterThan(0).WithMessage("Claim ID must be greater than 0.");

            // UpdatedBy validation rules (optional, but with length constraint)
            RuleFor(x => x.UpdatedBy)
                .MaximumLength(100).WithMessage("UpdatedBy must be at most 100 characters.");
        }
    }
}
