using FluentValidation;
using TsheThauLoo.Dtos.Account.Profile.Student;

namespace TsheThauLoo.Validator.Account.Profile
{
    public class StudentEditInfoDtoValidator : AbstractValidator<StudentEditInfoDto>
    {
        public StudentEditInfoDtoValidator()
        {
            RuleFor(x => x.NetworkId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("學號")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("networkId")
                .MaximumLength(10)
                .WithName("學號")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("networkId");
            RuleFor(x => x.College)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("學院")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("college")
                .MaximumLength(20)
                .WithName("學院")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("college");
            RuleFor(x => x.Department)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("系所")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("department")
                .MaximumLength(20)
                .WithName("系所")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("department");
            RuleFor(x => x.Class)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("班級")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("class")
                .MaximumLength(20)
                .WithName("班級")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("class");
        }
    }
}