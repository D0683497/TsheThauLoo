using FluentValidation;
using TsheThauLoo.Dtos.Account.Profile.Administrator;

namespace TsheThauLoo.Validator.Account.Profile
{
    public class AdministratorEditInfoDtoValidator : AbstractValidator<AdministratorEditInfoDto>
    {
        public AdministratorEditInfoDtoValidator()
        {
            RuleFor(x => x.ShowAbout)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithName("顯示於關於頁面")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("showAbout");
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
                .When(x => !string.IsNullOrEmpty(x.JobTitle))
                .NotEmpty()
                .WithName("職稱")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("jobTitle")
                .When(x => x.ShowAbout);
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
                .When(x => !string.IsNullOrEmpty(x.Extension))
                .NotEmpty()
                .WithName("分機")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("extension")
                .When(x => x.ShowAbout);
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
                .When(x => !string.IsNullOrEmpty(x.ContactEmail))
                .NotEmpty()
                .WithName("聯絡用電子郵件")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("contactEmail")
                .When(x => x.ShowAbout);
        }
    }
}