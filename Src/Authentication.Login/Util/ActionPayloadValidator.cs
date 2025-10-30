using Authentication.Login.DTO;
using Authentication.Login.Resource;
using FluentValidation;
using System.Diagnostics;

namespace Authentication.Login.Util
{
    /// <summary>
    /// FluentValidation validator for ActionPayLoadDTO objects.
    /// Defines validation rules for action creation and update requests.
    /// Ensures action name meets format and length requirements.
    /// </summary>
    [DebuggerDisplay("ActionPayloadValidator")]
    public class ActionPayloadValidator : AbstractValidator<ActionPayLoadDTO>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionPayloadValidator"/> class.
        /// Initializes validation rules for action payload.
        /// Validates action name format, length, and content restrictions.
        /// </summary>
        public ActionPayloadValidator()
        {
            // Name validation rules
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ResourceLogin.ActionNameRequired)
                .Must(n => !string.IsNullOrWhiteSpace(n) && !n.Contains(" ", StringComparison.Ordinal)).WithMessage(ResourceLogin.ActionNameCannotContainSpaces)
                .MinimumLength(1).WithMessage(ResourceLogin.ActionNameMustLeast1Character)
                .MaximumLength(100).WithMessage(ResourceLogin.ActionNameMustMost100Characters);

            // UpdatedBy validation rules (optional, but with length constraint)
            RuleFor(x => x.UpdatedBy)
                .MaximumLength(100).WithMessage(ResourceLogin.UpdatedByMustMost100Characters);
        }
    }
}
