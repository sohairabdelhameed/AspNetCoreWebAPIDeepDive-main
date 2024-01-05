using Microsoft.EntityFrameworkCore;

namespace CourseLibrary.API.Helpers
{
    // Generic class to handle paginated lists of items
    public class PagedList<T> : List<T>
    {
        // Properties to manage pagination information
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => (CurrentPage > 1);
        public bool HasNext => (CurrentPage < TotalPages);

        // Constructor to create a paginated list based on provided parameters
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize); // Calculate total pages
            AddRange(items); // Add items to the paginated list
        }

        // Method to create a paginated list asynchronously based on the IQueryable source
        public static async Task<PagedList<T>> CreateAsync(
            IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count(); // Get total count of items in the source
            var items = await source.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync(); // Retrieve items for the requested page
            return new PagedList<T>(items, count, pageNumber, pageSize); // Return a new PagedList instance
        }
    }
}
