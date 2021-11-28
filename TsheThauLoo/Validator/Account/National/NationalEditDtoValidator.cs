using System;
using FluentValidation;
using TsheThauLoo.Dtos.Account.National;
using TsheThauLoo.Utilities;

namespace TsheThauLoo.Validator.Account.National
{
    public class NationalEditDtoValidator : AbstractValidator<NationalEditDto>
    {
        public NationalEditDtoValidator()
        {
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