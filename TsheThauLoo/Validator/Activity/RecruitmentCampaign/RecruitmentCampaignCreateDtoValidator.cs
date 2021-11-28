using System;
using FluentValidation;
using TsheThauLoo.Dtos.Activity.RecruitmentCampaign;
using TsheThauLoo.Enums;

namespace TsheThauLoo.Validator.Activity.RecruitmentCampaign
{
    public class RecruitmentCampaignCreateDtoValidator : AbstractValidator<RecruitmentCampaignCreateDto>
    {
        public RecruitmentCampaignCreateDtoValidator()
        {
            RuleFor(x => x.Title)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("名稱")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("title")
                .MaximumLength(50)
                .WithName("名稱")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("title");
            RuleFor(x => x.Content)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("內容")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("content");
            RuleFor(x => x.StartDate)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithName("開始日期")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("startDate");
            RuleFor(x => x.StartTime)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithName("開始時間")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("startTime");
            RuleFor(x => x.EndDate)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithName("結束日期")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("endDate");
            RuleFor(x => x.EndTime)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithName("結束時間")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("endTime");
            RuleFor(x => x.EnableReview)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithName("審查")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("enableReview");
            
            // 活動開始日期 早於等於 活動結束日期
            When(x => true, () =>
            {
                RuleFor(x => new {x.StartDate, x.EndDate})
                    .Custom((parameters, context) =>
                    {
                        if (CompareDate(parameters.StartDate, parameters.EndDate))
                        {
                            context.AddFailure("endDate", "活動開始日期必須早於等於活動結束日期");
                        }
                    });
            });
        }
        
        private bool CompareDate(DateTime firstDate, DateTime secondDate)
        {
            DateTime date1 = new DateTime(firstDate.Year, firstDate.Month, firstDate.Day, 1, 1, 1);
            DateTime date2 = new DateTime(secondDate.Year, secondDate.Month, secondDate.Day, 1, 1, 1);
            var result = (TimeComparisonStatus) DateTime.Compare(date1, date2);
            switch (result)
            {
                case TimeComparisonStatus.Earlier:
                    return false;
                case TimeComparisonStatus.Same:
                    return false;
                case TimeComparisonStatus.Later:
                    return true;
                default:
                    return true;
            }
        }
    }
}