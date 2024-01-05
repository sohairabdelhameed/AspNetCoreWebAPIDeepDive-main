using CourseLibrary.API.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Models
{
    [CourseTitleMustBeDifferentFromDescription]
    public abstract class CourseForManipulationDto //: IValidatableObject
    {
        [Required(ErrorMessage = "You should fill out a title")]
        [MaxLength(100, ErrorMessage = "The title shouldnot have more than 100 character ")]
        public string Title { get; set; } = string.Empty;
        
        
        [MaxLength(1500, ErrorMessage = "The title shouldnot have more than 1500 character ")]
        public virtual string Description { get; set; } = string.Empty;

        //public IEnumerable<ValidationResult> 
        //    Validate(ValidationContext validationContext)
        //{ //we implement this method so we can add our business rule
        //    if (Title == Description)
        //    {
        //        //yield return makes sure that the validationResult is immediately returened after which code execution will continue
        //        //ValidationResult object is used to provide error message and related properties
        //        yield return new ValidationResult(
        //        "The provided description should be diffrent from the title."
        //         , new[] { " Course " });
            
        //    }
        //}
    }
}
