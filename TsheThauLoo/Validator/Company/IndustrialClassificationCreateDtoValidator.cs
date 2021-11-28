using FluentValidation;
using TsheThauLoo.Dtos.Company;

namespace TsheThauLoo.Validator.Company
{
    public class IndustrialClassificationCreateDtoValidator : AbstractValidator<IndustrialClassificationCreateDto>
    {
        public IndustrialClassificationCreateDtoValidator()
        {
            RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("說明")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("description")
                .MaximumLength(50)
                .WithName("說明")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("description");
        }
    }
}