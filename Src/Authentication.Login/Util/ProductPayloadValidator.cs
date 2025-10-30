using Authentication.Login.DTO;
using Authentication.Login.Resource;
using FluentValidation;

namespace Authentication.Login.Util
{
    public class ProductPayloadValidator : AbstractValidator<ProductPayLoadDTO>
    {
        public ProductPayloadValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ResourceLogin.NameIsRequired)
                .MaximumLength(100).WithMessage(ResourceLogin.NameMaxLength100);

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage(ResourceLogin.DescriptionMaxLength500);

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage(ResourceLogin.PriceMustBePositive);

            RuleFor(x => x.CreatedBy)
                .MaximumLength(100).WithMessage(ResourceLogin.CreatedByMaxLength100);

            RuleFor(x => x.UpdatedBy)
                .MaximumLength(100).WithMessage(ResourceLogin.UpdatedByMaxLength100);
        }
    }
}
