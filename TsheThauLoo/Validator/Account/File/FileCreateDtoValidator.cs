using FluentValidation;
using TsheThauLoo.Dtos.File;

namespace TsheThauLoo.Validator.Account.File
{
    public class FileCreateDtoValidator : AbstractValidator<FileCreateDto>
    {
        public FileCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithName("檔案名稱")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("name")
                .MaximumLength(270)
                .WithName("檔案名稱")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("name");
            RuleFor(x => x.Type)
                .NotEmpty()
                .WithName("檔案型態")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("type")
                .MaximumLength(130)
                .WithName("檔案型態")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("type");
            RuleFor(x => x.FileData)
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