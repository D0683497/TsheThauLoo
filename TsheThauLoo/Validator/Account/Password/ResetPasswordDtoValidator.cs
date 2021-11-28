using FluentValidation;
using TsheThauLoo.Dtos.Account.Password;
using TsheThauLoo.Utilities;

namespace TsheThauLoo.Validator.Account.Password
{
    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(x => x.UserId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("使用者識別碼")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("userId");
            RuleFor(x => x.Token)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("權杖")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("token");
            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("密碼")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("password")
                .Length(8, 64)
                .WithName("密碼")
                .WithMessage("{PropertyName}長度需介於{MinLength}到{MaxLength}之間")
                .OverridePropertyName("password")
                .Password();
            RuleFor(x => x.PasswordConfirm)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("確認密碼")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("passwordConfirm")
                .Length(8, 64)
                .WithName("確認密碼")
                .WithMessage("{PropertyName}長度需介於{MinLength}到{MaxLength}之間")
                .OverridePropertyName("passwordConfirm")
                .Equal(x => x.Password)
                .WithName("確認密碼")
                .WithMessage("{PropertyName}與密碼不相同")
                .OverridePropertyName("passwordConfirm");
        }
    }
}