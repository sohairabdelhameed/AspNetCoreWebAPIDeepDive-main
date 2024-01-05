namespace CourseLibrary.API.Models;

public class LinkDto
{
    public string? Href { get; private set; }//URI to be invoked
    public string? Rel { get; private set; }//type of action
    public string Method { get; private set; }// defines the method to use PUT,POST,GET...

    public LinkDto(string? href, string? rel, string method)
    {
        Href = href;
        Rel = rel;
        Method = method;
    }
}