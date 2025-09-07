using Authentication.Login.DTO;
using Authentication.Login.Resource;
using FluentValidation;

namespace Authentication.Login.Util
{
    /// <summary>
    /// FluentValidation validator for ActionPayLoadDTO objects.
    /// Defines validation rules for action creation and update requests.
    /// Ensures action name meets format and length requirements.
    /// </summary>
    public class ActionPayloadValidator : AbstractValidator<ActionPayLoadDTO>
    {
        /// <summary>
        /// Initializes validation rules for action payload.
        /// Validates action name format, length, and content restrictions.
        /// </summary>
        public ActionPayloadValidator()
        {
            // Name validation rules
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ResourceLogin.ActionNameRequired)
                .Must(n => !string.IsNullOrWhiteSpace(n) && !n.Contains(" ")).WithMessage(ResourceLogin.ActionNameCannotContainSpaces)
                .MinimumLength(1).WithMessage(ResourceLogin.ActionNameMustLeast1Character)
                .MaximumLength(100).WithMessage(ResourceLogin.ActionNameMustMost100Characters);

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