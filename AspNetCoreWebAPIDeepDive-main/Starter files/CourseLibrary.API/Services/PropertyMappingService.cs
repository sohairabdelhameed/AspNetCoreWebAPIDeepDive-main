using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;

namespace CourseLibrary.API.Services
{
    // Service for managing property mappings between different types
    public class PropertyMappingService : IPropertyMappingService
    {
        // Dictionary storing property mappings for AuthorDto to Author entity
        private readonly Dictionary<string, PropertyMappingValue> _authorPropertyMapping =
            new(StringComparer.OrdinalIgnoreCase)
            {
                { "Id", new PropertyMappingValue(new[] { "Id" }) },
                { "MainCategory", new PropertyMappingValue(new[] { "MainCategory" }) },
                { "Age", new PropertyMappingValue(new[] { "DateOfBirth" }, true) },
                { "Name", new PropertyMappingValue(new[] { "FirstName", "LastName" }) }
            };

        // List to store IPropertyMapping instances
        private readonly IList<IPropertyMapping> _propertyMappings =
            new List<IPropertyMapping>();

        // Constructor to set up initial property mappings
        public PropertyMappingService()
        {
            // Add property mappings for AuthorDto to Author entity
            _propertyMappings.Add(new PropertyMapping<AuthorDto, Author>(
                _authorPropertyMapping));
        }

        // Method to retrieve property mappings between source and destination types
        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            // Get the matching mapping from the list of property mappings
            var matchingMapping = _propertyMappings
                .OfType<PropertyMapping<TSource, TDestination>>();

            // Check if an exact match is found
            if (matchingMapping.Count() == 1)
            {
                // Return the mapping dictionary if a single match is found
                return matchingMapping.First().MappingDictionary;
            }

            // Throw an exception if an exact match is not found
            throw new Exception($"Cannot find exact property mapping instance " +
                $"for <{typeof(TSource)},{typeof(TDestination)}");
        }
        public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            // the string is separated by ",", so we split it.
            var fieldsAfterSplit = fields.Split(',');

            // run through the fields clauses
            foreach (var field in fieldsAfterSplit)
            {
                // trim
                var trimmedField = field.Trim();

                // remove everything after the first " " - if the fields 
                // are coming from an orderBy string, this part must be 
                // ignored
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedField : trimmedField.Remove(indexOfFirstSpace);

                // find the matching property
                if (!propertyMapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
