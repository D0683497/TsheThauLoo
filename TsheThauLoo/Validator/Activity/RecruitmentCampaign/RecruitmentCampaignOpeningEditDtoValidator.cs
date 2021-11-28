using FluentValidation;
using TsheThauLoo.Dtos.Activity.RecruitmentCampaign;

namespace TsheThauLoo.Validator.Activity.RecruitmentCampaign
{
    public class RecruitmentCampaignOpeningEditDtoValidator : AbstractValidator<RecruitmentCampaignOpeningEditDto>
    {
        public RecruitmentCampaignOpeningEditDtoValidator()
        {
            RuleFor(x => x.DivisionName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("職缺單位/部門")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("divisionName")
                .MaximumLength(30)
                .WithName("職缺單位/部門")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("divisionName");
            RuleFor(x => x.JobTitle)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("職務名稱")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("jobTitle")
                .MaximumLength(30)
                .WithName("職務名稱")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("jobTitle");
            RuleFor(x => x.JobDescription)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("工作內容")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("jobDescription");
            RuleFor(x => x.WorkPlace)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("工作地點")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("workPlace")
                .MaximumLength(200)
                .WithName("工作地點")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("workPlace");
            RuleFor(x => x.Salary)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("薪資")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("salary")
                .MaximumLength(20)
                .WithName("薪資")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("salary");
            RuleFor(x => x.RequiredNumber)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithName("需求人數")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("requiredNumber");
            RuleFor(x => x.Education)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithName("學歷")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("education")
                .IsInEnum()
                .WithName("學歷")
                .WithMessage("{PropertyName}格式錯誤")
                .OverridePropertyName("education");
            RuleFor(x => x.WorkExperience)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("相關工作經驗")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("workExperience")
                .MaximumLength(500)
                .WithName("相關工作經驗")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("workExperience");
            RuleFor(x => x.Language)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("語言能力")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("language")
                .MaximumLength(100)
                .WithName("語言能力")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("language");
            RuleFor(x => x.Nationality)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("聘用人員國籍")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("nationality")
                .MaximumLength(20)
                .WithName("聘用人員國籍")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("nationality");
            RuleFor(x => x.IsAccessibility)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithName("身心障礙者應徵")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("isAccessibility");
        }
    }
}