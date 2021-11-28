using FluentValidation;
using TsheThauLoo.Dtos.Account.Password;
using TsheThauLoo.Utilities;

namespace TsheThauLoo.Validator.Account.Password
{
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("目前密碼")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("currentPassword")
                .Length(8, 64)
                .WithName("目前密碼")
                .WithMessage("{PropertyName}長度需介於{MinLength}到{MaxLength}之間")
                .OverridePropertyName("currentPassword")
                .CurrentPassword();
            RuleFor(x => x.NewPassword)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("新密碼")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("newPassword")
                .Length(8, 64)
                .WithName("新密碼")
                .WithMessage("{PropertyName}長度需介於{MinLength}到{MaxLength}之間")
                .OverridePropertyName("newPassword")
                .NewPassword();
            RuleFor(x => x.ConfirmNewPassword)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("確認新密碼")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("confirmNewPassword")
                .Length(8, 64)
                .WithName("確認新密碼")
                .WithMessage("{PropertyName}長度需介於{MinLength}到{MaxLength}之間")
                .OverridePropertyName("confirmNewPassword")
                .Equal(x => x.NewPassword)
                .WithName("確認新密碼")
                .WithMessage("{PropertyName}與新密碼不相同")
                .OverridePropertyName("confirmNewPassword");
        }
    }
}