using FluentValidation;
using TreeWeb.Models.DTO;

namespace TreeWeb.Validation
{
    public class DirectoryDTOValidator : AbstractValidator<DirectoryDTO>
    {
        public DirectoryDTOValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100).Must(n => !n.Any(Path.GetInvalidFileNameChars().Contains));
        }
    }


}
