using FluentValidation;
using Tekton.Api.ViewModel.DTO;

namespace Tekton.Api.Validator
{
    public class ProductRequestUpdateDTOValidator : AbstractValidator<ProductRequestUpdateDTO>
    {
        public ProductRequestUpdateDTOValidator()
        {
            RuleFor(p => p.ProductId)
                        .GreaterThan(0).WithMessage("La propiedad {PropertyName} debe ser mayor a 0.");

            RuleFor(p => p.Name)
                         .NotEmpty().WithMessage("La propiedad {PropertyName} no debe ser vacia.")
                         .NotNull().WithMessage("La propiedad {PropertyName} no debe ser nula.");

            RuleFor(p => p.Stock)
                        .GreaterThanOrEqualTo(0).WithMessage("La propiedad {PropertyName} debe ser mayor o igual a 0.");

            RuleFor(p => p.Description)
                         .NotEmpty().WithMessage("La propiedad {PropertyName} no debe ser vacia.")
                         .NotNull().WithMessage("La propiedad {PropertyName} no debe ser nula.");

            RuleFor(p => p.Price)
                        .GreaterThanOrEqualTo(0).WithMessage("La propiedad {PropertyName} debe ser mayor o igual a 0.");
        }
    }
}