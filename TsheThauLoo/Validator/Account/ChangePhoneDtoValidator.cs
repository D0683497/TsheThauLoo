using FluentValidation;
using TsheThauLoo.Dtos.Account;
using TsheThauLoo.Utilities;

namespace TsheThauLoo.Validator.Account
{
    public class ChangePhoneDtoValidator : AbstractValidator<ChangePhoneDto>
    {
        public ChangePhoneDtoValidator()
        {
            RuleFor(x => x.NewPhoneNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("新的手機號碼")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("newPhoneNumber")
                .MaximumLength(30)
                .WithName("新的手機號碼")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("newPhoneNumber")
                .PhoneNumber()
                .WithName("新的手機號碼")
                .WithMessage("{PropertyName}格式錯誤")
                .OverridePropertyName("newPhoneNumber");
        }
    }
}