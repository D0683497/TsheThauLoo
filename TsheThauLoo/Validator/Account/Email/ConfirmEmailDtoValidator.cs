using FluentValidation;
using TsheThauLoo.Dtos.Account.Email;

namespace TsheThauLoo.Validator.Account.Email
{
    public class ConfirmEmailDtoValidator : AbstractValidator<ConfirmEmailDto>
    {
        public ConfirmEmailDtoValidator()
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
        }
    }
}