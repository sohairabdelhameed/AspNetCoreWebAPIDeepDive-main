namespace CourseLibrary.API.Models
{
    public class AuthorForCreationDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTimeOffset DateOfBirth { get; set; }
        public string MainCategory { get; set; } = string.Empty;
        
        //we created this here to avoid adding a course with the wrong authorId
        public ICollection <CourseForCreationDto> Courses { get; set; } 
        = new List<CourseForCreationDto>();
    }
}
