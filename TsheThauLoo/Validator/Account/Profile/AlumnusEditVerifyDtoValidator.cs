using FluentValidation;
using TsheThauLoo.Dtos.Account.Profile.Alumnus;

namespace TsheThauLoo.Validator.Account.Profile
{
    public class AlumnusEditVerifyDtoValidator : AbstractValidator<AlumnusEditVerifyDto>
    {
        public AlumnusEditVerifyDtoValidator()
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