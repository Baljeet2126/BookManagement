using System.ComponentModel.DataAnnotations;

    namespace BookManagement.API.Shared.Validations
    {
        public class HasNonEmptyStringsAttribute : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
            {
                if (value is not List<string> items || items == null)
                    return new ValidationResult($"{validationContext.DisplayName ?? validationContext.MemberName} is required.");

                if (items.Count == 0 || items.All(s => string.IsNullOrWhiteSpace(s)))
                    return new ValidationResult($"{validationContext.DisplayName ?? validationContext.MemberName} must contain at least one non-empty string.");

                return ValidationResult.Success;
            }
        }
    }

