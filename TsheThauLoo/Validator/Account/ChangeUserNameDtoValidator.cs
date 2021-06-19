using FluentValidation;
using TsheThauLoo.Dtos.Account;

namespace TsheThauLoo.Validator.Account
{
    public class ChangeUserNameDtoValidator : AbstractValidator<ChangeUserNameDto>
    {
        public ChangeUserNameDtoValidator()
        {
            RuleFor(x => x.NewUserName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("新的使用者名稱")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("newUserName")
                .MaximumLength(100)
                .WithName("新的使用者名稱")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("newUserName")
                .Matches(@"^[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789+\-=_.]+$")
                .WithName("新的使用者名稱")
                .WithMessage("{PropertyName}只能是字母或數字或 + - = _ .")
                .OverridePropertyName("newUserName");
        }
    }
}