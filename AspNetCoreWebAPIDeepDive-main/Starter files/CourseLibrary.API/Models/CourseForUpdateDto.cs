 using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Models
{
    public class CourseForUpdateDto : CourseForManipulationDto
    {
        [Required(ErrorMessage ="You should fill out the Description.")]
        public override string Description { get => base.Description; set => base.Description = value; }


    }
}
