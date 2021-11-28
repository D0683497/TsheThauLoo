using FluentValidation;
using TsheThauLoo.Dtos.File;

namespace TsheThauLoo.Validator.File
{
    public class FileEditDtoValidator : AbstractValidator<FileEditDto>
    {
        public FileEditDtoValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithName("檔案名稱")
                .WithMessage("{PropertyName}是必填的")
                .OverridePropertyName("name")
                .MaximumLength(260)
                .WithName("檔案名稱")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("name");
            RuleFor(x => x.Extension)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(10)
                .WithName("副檔名")
                .WithMessage("{PropertyName}最多{MaxLength}")
                .OverridePropertyName("extension")
                .When(x => !string.IsNullOrEmpty(x.Extension));
        }
    }
}