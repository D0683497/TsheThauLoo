using FluentValidation;
using TsheThauLoo.Dtos.Account.Profile.Administrator;

namespace TsheThauLoo.Validator.Account.Profile
{
    public class ResponsibilityEditDtoValidator : AbstractValidator<ResponsibilityEditDto>
    {
        public ResponsibilityEditDtoValidator()
        {
            RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("描述")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("description")
                .MaximumLength(200)
                .WithName("描述")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("description");
        }
    }
}