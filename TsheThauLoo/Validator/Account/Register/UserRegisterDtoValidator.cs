using System;
using FluentValidation;
using TsheThauLoo.Dtos.Account.Register;
using TsheThauLoo.Utilities;

namespace TsheThauLoo.Validator.Account.Register
{
    public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterDtoValidator()
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
            RuleFor(x => x.PhoneNumber)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(30)
                .WithName("手機號碼")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("phoneNumber")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
                .PhoneNumber()
                .WithName("手機號碼")
                .WithMessage("{PropertyName}格式錯誤")
                .OverridePropertyName("phoneNumber")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
            RuleFor(x => x.NationalId)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(10)
                .WithName("身份證字號")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("nationalId")
                .When(x => !string.IsNullOrEmpty(x.NationalId))
                .NationalId()
                .When(x => !string.IsNullOrEmpty(x.NationalId));
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("姓名")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("name")
                .MaximumLength(50)
                .WithName("姓名")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("name");
            RuleFor(x => x.Gender)
                .Cascade(CascadeMode.Stop)
                .IsInEnum()
                .WithName("性別")
                .WithMessage("{PropertyName}格式錯誤")
                .OverridePropertyName("gender")
                .When(x => x.Gender != null);
            RuleFor(x => x.DateOfBirth)
                .Cascade(CascadeMode.Stop)
                .LessThan(DateTime.Now)
                .WithName("生日")
                .WithMessage("{PropertyName}不能晚於今天")
                .OverridePropertyName("dateOfBirth")
                .When(x => x.DateOfBirth != null);
            RuleFor(x => x.CurrentAddress)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(200)
                .WithName("通訊地址")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("name")
                .When(x => !string.IsNullOrEmpty(x.CurrentAddress));
        }
    }
}