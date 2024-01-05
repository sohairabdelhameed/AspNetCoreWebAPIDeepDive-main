using CourseLibrary.API.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.ValidationAttributes
{
    // Custom validation attribute that ensures the Title of a course is different from its Description
    public class CourseTitleMustBeDifferentFromDescriptionAttribute : ValidationAttribute
    {
        // Constructor 
        public CourseTitleMustBeDifferentFromDescriptionAttribute()
        {
        }

        // Override of the IsValid method to perform custom validation
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Check if the object instance being validated is of type CourseForManipulationDto
            if (validationContext.ObjectInstance is not CourseForManipulationDto course)
            {
                // Throw an exception if the attribute isn't applied to the correct type
                throw new Exception($"Attribute " +
                    $"{nameof(CourseTitleMustBeDifferentFromDescriptionAttribute)} " +
                    $"must be applied to a " +
                    $"{nameof(CourseForManipulationDto)} or derived type.");
            }

            // Check if the Title property of the CourseForManipulationDto is the same as the Description property
            if (course.Title == course.Description)
            {
                // If they are the same, return a ValidationResult with an error message
                return new ValidationResult(
                    "The provided description should be different from the title.",
                    new[] { nameof(CourseForManipulationDto) });
            }

            // If Title and Description are different, return ValidationResult.Success
            // indicating successful validation
            return ValidationResult.Success;
        }
    }
}
