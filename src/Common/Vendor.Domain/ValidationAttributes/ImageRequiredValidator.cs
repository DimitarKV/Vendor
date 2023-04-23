using System.ComponentModel.DataAnnotations;

namespace Vendor.Domain.ValidationAttributes;

public class ImageRequiredValidator : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not null)
            return null;
        return new ValidationResult("Form file is required", new[] { validationContext.MemberName });
    }
}