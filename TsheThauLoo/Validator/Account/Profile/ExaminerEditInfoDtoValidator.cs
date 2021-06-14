using FluentValidation;
using TsheThauLoo.Dtos.Account.Profile.Examiner;

namespace TsheThauLoo.Validator.Account.Profile
{
    public class ExaminerEditInfoDtoValidator : AbstractValidator<ExaminerEditInfoDto>
    {
        public ExaminerEditInfoDtoValidator()
        {
            RuleFor(x => x.DivisionName)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(30)
                .WithName("工作單位")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("divisionName")
                .When(x => !string.IsNullOrEmpty(x.DivisionName));
            RuleFor(x => x.JobTitle)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(30)
                .WithName("職稱")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("jobTitle")
                .When(x => !string.IsNullOrEmpty(x.JobTitle));
        }
    }
}