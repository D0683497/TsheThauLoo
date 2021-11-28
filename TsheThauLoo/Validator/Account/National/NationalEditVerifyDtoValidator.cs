using FluentValidation;
using TsheThauLoo.Dtos.Account.National;

namespace TsheThauLoo.Validator.Account.National
{
    public class NationalEditVerifyDtoValidator : AbstractValidator<NationalEditVerifyDto>
    {
        public NationalEditVerifyDtoValidator()
        {
            RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(500)
                .WithName("描述")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("description")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}