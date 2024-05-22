using System.ComponentModel.DataAnnotations;

namespace FurnitureStore.Contracts.ValidationAttributes;

public class MinimumValueAttribute : ValidationAttribute
{
    private readonly decimal _minValue;

    public MinimumValueAttribute(double minValue)
    {
        _minValue = (decimal)minValue;
    }


    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value != null && (decimal)value < _minValue)
        {
            return new ValidationResult($"The field {validationContext.DisplayName} must be greater than or equal to {_minValue}.");
        }

        return ValidationResult.Success;
    }
}
