using FluentValidation;
using TsheThauLoo.Dtos.Account.Register;

namespace TsheThauLoo.Validator.Account.Register
{
    public class AlumnusRegisterDtoValidator : AbstractValidator<AlumnusRegisterDto>
    {
        public AlumnusRegisterDtoValidator()
        {
            Include(new UserRegisterDtoValidator());

            RuleFor(x => x.DateOfGraduation)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(10)
                .WithName("畢業年度")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("dateOfGraduation")
                .When(x => !string.IsNullOrEmpty(x.DateOfGraduation));
            RuleFor(x => x.College)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("畢業學院")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("college")
                .MaximumLength(20)
                .WithName("畢業學院")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("college");
            RuleFor(x => x.Department)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("畢業系所")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("department")
                .MaximumLength(20)
                .WithName("畢業系所")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("department");
            RuleFor(x => x.Class)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("畢業班級")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("class")
                .MaximumLength(20)
                .WithName("畢業班級")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("class");
        }
    }
}