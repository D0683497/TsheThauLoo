using System;
using FluentValidation;
using TsheThauLoo.Dtos.Manage;
using TsheThauLoo.Utilities;

namespace TsheThauLoo.Validator.Manage
{
    public class AlumnusManageDtoValidator : AbstractValidator<AlumnusManageDto>
    {
        public AlumnusManageDtoValidator()
        {
            #region UserManage

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
            RuleFor(x => x.EmailConfirmed)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithName("電子郵件驗證")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("emailConfirmed");
            RuleFor(x => x.PhoneNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("手機號碼")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("phoneNumber")
                .When(x => x.PhoneNumberConfirmed)
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
            RuleFor(x => x.PhoneNumberConfirmed)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithName("手機號碼驗證")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("phoneNumberConfirmed");
            RuleFor(x => x.LockoutEnd);
            RuleFor(x => x.LockoutEnabled)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithName("可以鎖定使用者")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("lockoutEnabled");
            RuleFor(x => x.AccessFailedCount)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithName("登入失敗次數")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("accessFailedCount");
            RuleFor(x => x.IsEnable)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithName("啟用帳戶")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("isEnable");
            RuleFor(x => x.IdentityConfirmed)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithName("實名驗證")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("identityConfirmed");
            RuleFor(x => x.NationalId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("身份證字號")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("nationalId")
                .When(x => x.IdentityConfirmed)
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
                .NotNull()
                .WithName("性別")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("gender")
                .When(x => x.IdentityConfirmed)
                .IsInEnum()
                .WithName("性別")
                .WithMessage("{PropertyName}格式錯誤")
                .OverridePropertyName("gender")
                .When(x => x.Gender != null);
            RuleFor(x => x.DateOfBirth)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithName("生日")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("dateOfBirth")
                .When(x => x.IdentityConfirmed)
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

            #endregion

            #region AlumnusManage

            RuleFor(x => x.DateOfGraduation)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(10)
                .WithName("畢業年度")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("dateOfGraduation")
                .When(x => !string.IsNullOrEmpty(x.DateOfGraduation));
            RuleFor(x => x.College)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("畢業學院")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("college")
                .MaximumLength(20)
                .WithName("畢業學院")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("college");
            RuleFor(x => x.Department)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("畢業系所")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("department")
                .MaximumLength(20)
                .WithName("畢業系所")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("department");
            RuleFor(x => x.Class)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("畢業班級")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("class")
                .MaximumLength(20)
                .WithName("畢業班級")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("class");

            #endregion
        }
    }
}