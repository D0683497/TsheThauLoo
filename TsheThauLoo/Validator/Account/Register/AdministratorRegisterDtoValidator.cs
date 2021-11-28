using FluentValidation;
using TsheThauLoo.Dtos.Account.Register;

namespace TsheThauLoo.Validator.Account.Register
{
    public class AdministratorRegisterDtoValidator : AbstractValidator<AdministratorRegisterDto>
    {
        public AdministratorRegisterDtoValidator()
        {
            Include(new UserRegisterDtoValidator());
            
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
                .WithName("部門")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("dept")
                .MaximumLength(20)
                .WithName("部門")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("dept");
            RuleFor(x => x.Unit)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(20)
                .WithName("單位")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("unit")
                .When(x => !string.IsNullOrEmpty(x.Unit));
            RuleFor(x => x.JobTitle)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(20)
                .WithName("職稱")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("jobTitle")
                .When(x => !string.IsNullOrEmpty(x.JobTitle));
            RuleFor(x => x.Extension)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(10)
                .WithName("分機")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("extension")
                .When(x => !string.IsNullOrEmpty(x.Extension))
                .Matches(@"^[0123456789]+$")
                .WithName("分機")
                .WithMessage("{PropertyName}只能是數字")
                .OverridePropertyName("extension")
                .When(x => !string.IsNullOrEmpty(x.Extension));
            RuleFor(x => x.ContactEmail)
                .Cascade(CascadeMode.Stop)
                .EmailAddress()
                .WithName("聯絡用電子郵件")
                .WithMessage("{PropertyName}格式錯誤")
                .OverridePropertyName("contactEmail")
                .When(x => !string.IsNullOrEmpty(x.ContactEmail))
                .MaximumLength(320)
                .WithName("聯絡用電子郵件")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("contactEmail")
                .When(x => !string.IsNullOrEmpty(x.ContactEmail));
        }
    }
}