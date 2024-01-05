namespace CourseLibrary.API.Services
{
    // Class representing property mapping information
    public class PropertyMappingValue
    {
        // Collection of destination properties
        public IEnumerable<string> DestinationProperties { get; private set; }

        // Indicates whether the mapping should be reverted
        public bool Revert { get; private set; }

        // Constructor to initialize destination properties and revert flag
        public PropertyMappingValue(IEnumerable<string> destinationProperties, bool revert = false)
        {
            // Assign destination properties or throw an ArgumentNullException if null
            DestinationProperties = destinationProperties
                ?? throw new ArgumentNullException(nameof(destinationProperties));

            // Set the revert flag
            Revert = revert;
        }
    }
}
