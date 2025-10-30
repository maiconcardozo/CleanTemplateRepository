using Authentication.Login.DTO;
using Authentication.Login.Resource;
using FluentValidation;

namespace Authentication.Login.Util
{
    public class ProductVariantPayloadValidator : AbstractValidator<ProductVariantPayLoadDTO>
    {
        public ProductVariantPayloadValidator()
        {
            RuleFor(x => x.IdProduct)
                .GreaterThan(0).WithMessage(ResourceLogin.IdProductIsRequired);

            RuleFor(x => x.SKU)
                .NotEmpty().WithMessage(ResourceLogin.SKUIsRequired)
                .MaximumLength(50).WithMessage(ResourceLogin.SKUMaxLength50);

            RuleFor(x => x.Color)
                .MaximumLength(50).WithMessage(ResourceLogin.ColorMaxLength50);

            RuleFor(x => x.Size)
                .MaximumLength(20).WithMessage(ResourceLogin.SizeMaxLength20);

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage(ResourceLogin.StockQuantityMustBePositive);

            RuleFor(x => x.CreatedBy)
                .MaximumLength(100).WithMessage(ResourceLogin.CreatedByMaxLength100);

            RuleFor(x => x.UpdatedBy)
                .MaximumLength(100).WithMessage(ResourceLogin.UpdatedByMaxLength100);
        }
    }
}
