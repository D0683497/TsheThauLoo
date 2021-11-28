using FluentValidation;
using TsheThauLoo.Dtos.Company;
using TsheThauLoo.Utilities;

namespace TsheThauLoo.Validator.Company
{
    public class CompanyCreateDtoValidator : AbstractValidator<CompanyCreateDto>
    {
        public CompanyCreateDtoValidator()
        {
            RuleFor(x => x.RegistrationNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("統一編號")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("registrationNumber")
                .MaximumLength(10)
                .WithName("統一編號")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("registrationNumber")
                .RegistrationNumber();
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("名稱")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("name")
                .MaximumLength(100)
                .WithName("名稱")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("name");
            RuleFor(x => x.Introduction);
            RuleFor(x => x.Website)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(300)
                .WithName("網站")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("website")
                .When(x => x.Website != null)
                .Matches(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)")
                .WithName("網站")
                .WithMessage("{PropertyName}格式錯誤")
                .OverridePropertyName("website")
                .When(x => x.Website != null);
        }
    }
}