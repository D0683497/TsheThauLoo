using FluentValidation;
using TsheThauLoo.Dtos.Account.Password;

namespace TsheThauLoo.Validator.Account.Password
{
    public class ForgetPasswordDtoValidator : AbstractValidator<ForgetPasswordDto>
    {
        public ForgetPasswordDtoValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("電子郵件")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("email")
                .EmailAddress()
                .WithName("電子郵件")
                .WithMessage("{PropertyName}格式錯誤")
                .OverridePropertyName("email")
                .MaximumLength(320)
                .WithName("電子郵件")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("email");
        }
    }
}