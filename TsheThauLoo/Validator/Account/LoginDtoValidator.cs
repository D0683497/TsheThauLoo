using FluentValidation;
using TsheThauLoo.Dtos.Account.Login;
using TsheThauLoo.Utilities;

namespace TsheThauLoo.Validator.Account
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.UserName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("使用者名稱")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("userName")
                .MaximumLength(100)
                .WithName("使用者名稱")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("userName")
                .Matches(@"^[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789+\-=_.]+$")
                .WithName("使用者名稱")
                .WithMessage("{PropertyName}只能是字母或數字或 + - = _ .")
                .OverridePropertyName("userName");
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
        }
    }
}