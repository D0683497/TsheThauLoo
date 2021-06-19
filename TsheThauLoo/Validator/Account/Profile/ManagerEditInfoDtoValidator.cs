using FluentValidation;
using TsheThauLoo.Dtos.Account.Profile.Manager;
using TsheThauLoo.Utilities;

namespace TsheThauLoo.Validator.Account.Profile
{
    public class ManagerEditInfoDtoValidator : AbstractValidator<ManagerEditInfoDto>
    {
        public ManagerEditInfoDtoValidator()
        {
            RuleFor(x => x.DivisionName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("所屬部門/單位")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("divisionName")
                .MaximumLength(30)
                .WithName("所屬部門/單位")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("divisionName");
            RuleFor(x => x.JobTitle)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("職稱")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("jobTitle")
                .MaximumLength(30)
                .WithName("職稱")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("jobTitle");
            RuleFor(x => x.ContactEmail)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("聯絡用電子郵件")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("contactEmail")
                .EmailAddress()
                .WithName("聯絡用電子郵件")
                .WithMessage("{PropertyName}格式錯誤")
                .OverridePropertyName("contactEmail")
                .MaximumLength(320)
                .WithName("聯絡用電子郵件")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("contactEmail");
            RuleFor(x => x.ContactPhone)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("聯絡用電話號碼")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("contactPhone")
                .MaximumLength(30)
                .WithName("聯絡用電話號碼")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("contactPhone")
                .PhoneNumber()
                .WithName("聯絡用電話號碼")
                .WithMessage("{PropertyName}格式錯誤")
                .OverridePropertyName("contactPhone");
            RuleFor(x => x.ContactAddress)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("聯絡用地址")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("contactAddress")
                .MaximumLength(200)
                .WithName("聯絡用地址")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("contactAddress");
        }
    }
}