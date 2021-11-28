using FluentValidation;
using TsheThauLoo.Dtos.Activity.RecruitmentCampaign;

namespace TsheThauLoo.Validator.Activity.RecruitmentCampaign
{
    public class FacultyCreateDtoValidator : AbstractValidator<FacultyCreateDto>
    {
        public FacultyCreateDtoValidator()
        {
            RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("描述")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("description")
                .MaximumLength(50)
                .WithName("描述")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("description");
        }
    }
}