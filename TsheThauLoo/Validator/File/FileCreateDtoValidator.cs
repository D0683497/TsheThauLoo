using FluentValidation;
using TsheThauLoo.Dtos.File;

namespace TsheThauLoo.Validator.File
{
    public class FileCreateDtoValidator : AbstractValidator<FileCreateDto>
    {
        public FileCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("檔案名稱")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("name")
                .MaximumLength(270)
                .WithName("檔案名稱")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("name");
            RuleFor(x => x.Type)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("檔案型態")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("type")
                .MaximumLength(130)
                .WithName("檔案型態")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("type");
            RuleFor(x => x.FileData)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithName("檔案")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("fileData")
                .Custom((parameters, context) =>
                {
                    // 約 2 GB
                    if (parameters.Length > int.MaxValue)
                    {
                        context.AddFailure("fileData", "檔案過大");
                    }
                });;
        }
    }
}