using System;
using FluentValidation;
using TsheThauLoo.Dtos.Activity.GeneralCampaign;
using TsheThauLoo.Enums;

namespace TsheThauLoo.Validator.Activity.GeneralCampaign
{
    public class GeneralCampaignEditDtoValidator : AbstractValidator<GeneralCampaignEditDto>
    {
        public GeneralCampaignEditDtoValidator()
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
            RuleFor(x => x.Declaration);
            RuleFor(x => x.Venue)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(200)
                .WithName("地點")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("venue")
                .When(x => !string.IsNullOrEmpty(x.Venue));
            RuleFor(x => x.RegistrationStartDate)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithName("報名開始日期")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("registrationStartDate")
                .When(x => x.RegistrationStartTime != null || x.RegistrationEndDate != null);
            RuleFor(x => x.RegistrationStartTime)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithName("報名開始時間")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("registrationStartTime")
                .When(x => x.RegistrationStartDate != null || x.RegistrationEndTime != null);
            RuleFor(x => x.RegistrationEndDate)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithName("報名結束日期")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("registrationEndDate")
                .When(x => x.RegistrationEndTime != null || x.RegistrationStartDate != null);
            RuleFor(x => x.RegistrationEndTime)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithName("報名結束時間")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("registrationEndTime")
                .When(x => x.RegistrationEndDate != null || x.RegistrationStartTime != null);
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
            RuleFor(x => x.LimitNumberOfPeople)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithName("人數限制")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("limitNumberOfPeople");
            RuleFor(x => x.EnableVerify)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithName("審核")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("enableVerify");
            RuleFor(x => x.EnableIdentityConfirmed)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithName("實名審核")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("enableIdentityConfirmed");
            
            // 活動報名開始日期 早於等於 活動報名結束日期
            When(x => x.RegistrationStartDate != null && x.RegistrationEndDate != null, () =>
            {
                RuleFor(x => new {x.RegistrationStartDate, x.RegistrationEndDate})
                    .Custom((parameters, context) =>
                    {
                        if (CompareDate((DateTime) parameters.RegistrationStartDate, (DateTime) parameters.RegistrationEndDate))
                        {
                            context.AddFailure("registrationEndDate", "活動報名開始日期必須早於等於活動報名結束日期");
                        }
                    });
            });

            // 活動報名結束日期 早於等於 活動開始日期
            When(x => x.RegistrationEndDate != null && x.StartDate != null, () =>
            {
                RuleFor(x => new {x.RegistrationEndDate, x.StartDate})
                    .Custom((parameters, context) =>
                    {
                        if (CompareDate((DateTime) parameters.RegistrationEndDate, parameters.StartDate))
                        {
                            context.AddFailure("startDate", "活動報名結束日期必須早於等於活動開始日期");
                        }
                    });
            });

            // 活動開始日期 早於等於 活動結束日期
            When(x => x.StartDate != null && x.EndDate != null, () =>
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