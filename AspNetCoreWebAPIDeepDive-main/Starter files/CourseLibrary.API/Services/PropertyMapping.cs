namespace CourseLibrary.API.Services
{
    // Class representing property mapping between source and destination types
    public class PropertyMapping<TSource, TDestination> : IPropertyMapping
    {
        // Dictionary storing property mappings between source and destination properties
        public Dictionary<string, PropertyMappingValue> MappingDictionary { get; private set; }

        // Constructor to initialize property mapping dictionary
        public PropertyMapping(Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            // Assign the mapping dictionary or throw an ArgumentNullException if null
            MappingDictionary = mappingDictionary
                ?? throw new ArgumentNullException(nameof(mappingDictionary));
        }
    }
}
