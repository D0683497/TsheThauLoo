using FluentValidation;
using TsheThauLoo.Dtos.Account.Profile.Employee;

namespace TsheThauLoo.Validator.Account.Profile
{
    public class EmployeeEditInfoDtoValidator : AbstractValidator<EmployeeEditInfoDto>
    {
        public EmployeeEditInfoDtoValidator()
        {
            RuleFor(x => x.NetworkId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("證號")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("networkId")
                .MaximumLength(10)
                .WithName("證號")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("networkId");
            RuleFor(x => x.Dept)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("部門(學院)")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("dept")
                .MaximumLength(20)
                .WithName("部門(學院)")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("dept");
            RuleFor(x => x.Unit)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(20)
                .WithName("單位(系所)")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("unit")
                .When(x => !string.IsNullOrEmpty(x.Unit));
        }
    }
}