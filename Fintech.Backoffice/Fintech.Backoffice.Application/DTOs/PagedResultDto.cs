namespace Fintech.Backoffice.Application.DTOs
{
    /// <summary>
    /// Generic DTO for paginated results.
    /// Wraps paginated data with metadata about pagination.
    /// Why: Generic<T> allows reusing this for any collection of DTOs,
    /// avoiding code duplication for different entity types.
    /// </summary>
    /// <typeparam name="T">Type of items in the result list</typeparam>
    public class PagedResultDto<T> where T : class
    {
        /// <summary>
        /// The collection of items for this page.
        /// </summary>
        public IEnumerable<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// Total number of items across all pages (not just this page).
        /// Used to calculate total pages.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Current page number (1-based).
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Number of items per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total number of pages.
        /// Calculated as: (TotalCount + PageSize - 1) / PageSize
        /// This formula handles remainder correctly (always rounds up).
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((decimal)TotalCount / PageSize);

        /// <summary>
        /// Whether there is a next page.
        /// True if current page < total pages.
        /// </summary>
        public bool HasNextPage => PageNumber < TotalPages;

        /// <summary>
        /// Whether there is a previous page.
        /// True if current page > 1.
        /// </summary>
        public bool HasPreviousPage => PageNumber > 1;

        /// <summary>
        /// Creates a new PagedResultDto with the given data.
        /// </summary>
        /// <param name="items">Collection of items for this page</param>
        /// <param name="totalCount">Total count across all pages</param>
        /// <param name="pageNumber">Current page number</param>
        /// <param name="pageSize">Page size</param>
        public PagedResultDto(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public PagedResultDto()
        {
        }
    }
}
