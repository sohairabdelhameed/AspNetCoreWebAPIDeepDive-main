using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace CourseLibrary.API.ActionConstraints
{
    // Custom attribute for checking if the request header matches specified media types
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class RequestHeaderMatchesMediaTypeAttribute : Attribute, IActionConstraint
    {
        private readonly string _requestHeaderToMatch;
        private readonly MediaTypeCollection _mediaTypes = new();

        // Constructor accepting the header name to match and one or more media types
        public RequestHeaderMatchesMediaTypeAttribute(string requestHeaderToMatch,
              string mediaType, params string[] otherMediaTypes)
        {
            _requestHeaderToMatch = requestHeaderToMatch
                    ?? throw new ArgumentNullException(nameof(requestHeaderToMatch));

            // Check if the inputted media types are valid media types and add them to the _mediaTypes collection

            if (MediaTypeHeaderValue.TryParse(mediaType, out var parsedMediaType))
            {
                _mediaTypes.Add(parsedMediaType);
            }
            else
            {
                throw new ArgumentException(nameof(mediaType));
            }

            foreach (var otherMediaType in otherMediaTypes)
            {
                if (MediaTypeHeaderValue.TryParse(otherMediaType, out var parsedOtherMediaType))
                {
                    _mediaTypes.Add(parsedOtherMediaType);
                }
                else
                {
                    throw new ArgumentException(nameof(otherMediaTypes));
                }
            }
        }

        public int Order { get; }

        // Method to determine if the request's media type matches any of the specified media types
        public bool Accept(ActionConstraintContext context)
        {
            var requestHeaders = context.RouteContext.HttpContext.Request.Headers;

            // Check if the request contains the specified header
            if (!requestHeaders.ContainsKey(_requestHeaderToMatch))
            {
                return false;
            }

            // Parse the request's media type
            var parsedRequestMediaType = new MediaType(requestHeaders[_requestHeaderToMatch]);

            // Check if the request's media type matches any of the specified media types
            foreach (var mediaType in _mediaTypes)
            {
                var parsedMediaType = new MediaType(mediaType);
                if (parsedRequestMediaType.Equals(parsedMediaType))
                {
                    return true; // If a match is found, return true
                }
            }
            return false; // If no match is found, return false
        }
    }
}
