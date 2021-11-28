using FluentValidation;
using TsheThauLoo.Dtos.Account.Email;

namespace TsheThauLoo.Validator.Account.Email
{
    public class ChangeEmailDtoValidator : AbstractValidator<ChangeEmailDto>
    {
        public ChangeEmailDtoValidator()
        {
            RuleFor(x => x.NewEmail)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("新的電子郵件")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("newEmail")
                .EmailAddress()
                .WithName("新的電子郵件")
                .WithMessage("{PropertyName}格式錯誤")
                .OverridePropertyName("newEmail")
                .MaximumLength(320)
                .WithName("新的電子郵件")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("newEmail");
        }
    }
}