using Authentication.Login.DTO;
using FluentValidation;

namespace Authentication.Login.Util
{
    /// <summary>
    /// FluentValidation validator for ApplicationPayLoadDTO objects.
    /// Defines validation rules for application creation and update requests.
    /// Ensures application name and description meet format and length requirements.
    /// </summary>
    public class ApplicationPayloadValidator : AbstractValidator<ApplicationPayLoadDTO>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationPayloadValidator"/> class.
        /// Initializes validation rules for application payload.
        /// Validates application name and description format, length, and content restrictions.
        /// </summary>
        public ApplicationPayloadValidator()
        {
            // Name validation rules
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Application name is required.")
                .Must(n => !string.IsNullOrWhiteSpace(n) && !n.Contains(" ", StringComparison.Ordinal)).WithMessage("Application name cannot contain spaces or be null/empty.")
                .MinimumLength(1).WithMessage("Application name must be at least 1 character.")
                .MaximumLength(100).WithMessage("Application name must be at most 100 characters.");

            // Description validation rules
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Application description is required.")
                .MinimumLength(1).WithMessage("Application description must be at least 1 character.")
                .MaximumLength(500).WithMessage("Application description must be at most 500 characters.");

            // UpdatedBy validation rules (optional, but with length constraint)
            RuleFor(x => x.UpdatedBy)
                .MaximumLength(100).WithMessage("UpdatedBy must be at most 100 characters.");
        }
    }
}
