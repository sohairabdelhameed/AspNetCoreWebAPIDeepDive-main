using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authorcollections")]
    public class AuthorCollectionController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository; 
        private readonly IMapper _mapper;
        
        //Dependendency Injection
        public AuthorCollectionController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository??
                throw new ArgumentException(nameof(courseLibraryRepository));

            _mapper = mapper ??
                throw new ArgumentException(nameof(mapper));
            
        }
        //Creating Actions
        [HttpGet("({authorIds})")]
        public async Task<ActionResult<IEnumerable<AuthorForCreationDto>>>
            GetAuthorCollection([FromRoute] IEnumerable<Guid> authorIds)
        {

        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> CreateAuthorCollection(
            IEnumerable<AuthorForCreationDto> authorCollection)
        {
            var authorEntities = _mapper.Map<IEnumerable<Author>>(authorCollection);
            foreach (var author in authorEntities)
            {
                _courseLibraryRepository.AddAuthor(author);

            }
            await _courseLibraryRepository.SaveAsync();

            return Ok();
        }

    }
}
 