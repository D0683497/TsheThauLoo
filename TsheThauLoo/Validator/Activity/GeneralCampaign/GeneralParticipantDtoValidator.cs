using FluentValidation;
using TsheThauLoo.Dtos.Activity.GeneralCampaign;
using TsheThauLoo.Utilities;

namespace TsheThauLoo.Validator.Activity.GeneralCampaign
{
    public class GeneralParticipantDtoValidator : AbstractValidator<GeneralParticipantDto>
    {
        public GeneralParticipantDtoValidator()
        {
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
            RuleFor(x => x.ContactPhone)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("聯絡用電話號碼")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("contactPhone")
                .MaximumLength(30)
                .WithName("聯絡用電話號碼")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("contactPhone")
                .PhoneNumber()
                .WithName("聯絡用電話號碼")
                .WithMessage("{PropertyName}格式錯誤")
                .OverridePropertyName("contactPhone");
            RuleFor(x => x.Remark)
                .MaximumLength(500)
                .WithName("備註")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("remark")
                .When(x => !string.IsNullOrEmpty(x.Remark));
        }
    }
}