namespace Fintech.Backoffice.Application.DTOs
{
    /// <summary>
    /// DTO for filtering and searching applications.
    /// Encapsulates all filter criteria into a single object.
    /// Why: Using a filter DTO instead of multiple parameters makes the API cleaner,
    /// easier to test, and allows adding new filters without changing method signatures.
    /// </summary>
    public class ApplicationFilterDto
    {
        // ============================================================================
        // Pagination Properties
        // ============================================================================

        /// <summary>
        /// Current page number (1-based).
        /// Default: 1
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Number of records per page.
        /// Default: 10, Max: 100 (enforced in service)
        /// </summary>
        public int PageSize { get; set; } = 10;

        // ============================================================================
        // Filter Properties
        // ============================================================================

        /// <summary>
        /// Filter by customer name (partial match, case-insensitive).
        /// Optional - null means no customer name filter.
        /// </summary>
        public string? CustomerName { get; set; }

        /// <summary>
        /// Filter by process number (partial match).
        /// Example: "APP-2024" would match "APP-2024-001001", "APP-2024-001002", etc.
        /// </summary>
        public string? ProcessNumber { get; set; }

        /// <summary>
        /// Filter by partner ID (exact match).
        /// Null means no partner filter.
        /// </summary>
        public int? PartnerId { get; set; }

        /// <summary>
        /// Filter by application status code.
        /// Values: 1=InProgress, 2=Rejected, 3=Completed, 4=Cancelled
        /// Null means no status filter (show all statuses).
        /// </summary>
        public int? StatusCode { get; set; }

        /// <summary>
        /// Filter applications from this date onwards (inclusive).
        /// Null means no start date filter.
        /// </summary>
        public DateTime? ApplicationDateFrom { get; set; }

        /// <summary>
        /// Filter applications up to this date (inclusive).
        /// Null means no end date filter.
        /// </summary>
        public DateTime? ApplicationDateTo { get; set; }

        /// <summary>
        /// Filter by minimum approved amount.
        /// Null means no minimum filter.
        /// </summary>
        public decimal? ApprovedAmountMin { get; set; }

        /// <summary>
        /// Filter by maximum approved amount.
        /// Null means no maximum filter.
        /// </summary>
        public decimal? ApprovedAmountMax { get; set; }

        // ============================================================================
        // Sorting Properties
        // ============================================================================

        /// <summary>
        /// Field to sort by.
        /// Valid values: "ProcessNumber", "CustomerName", "ApprovedAmount", "Status", "ApplicationDate", "Partner"
        /// Default: "ApplicationDate"
        /// </summary>
        public string SortBy { get; set; } = "ApplicationDate";

        /// <summary>
        /// Sort order direction.
        /// Valid values: "asc" (ascending), "desc" (descending)
        /// Default: "desc" (newest first)
        /// </summary>
        public string SortOrder { get; set; } = "desc";

        // ============================================================================
        // Validation and Utility Methods
        // ============================================================================

        /// <summary>
        /// Validates the filter object to ensure all values are within acceptable ranges.
        /// Returns a list of validation errors, or empty list if valid.
        /// Why: Keeping validation with the DTO makes it easy to reuse validation logic.
        /// </summary>
        /// <returns>List of validation errors</returns>
        public List<string> Validate()
        {
            var errors = new List<string>();

            // Validate pagination
            if (PageNumber < 1)
                errors.Add("PageNumber must be >= 1");

            if (PageSize < 1 || PageSize > 100)
                errors.Add("PageSize must be between 1 and 100");

            // Validate date range
            if (ApplicationDateFrom.HasValue && ApplicationDateTo.HasValue)
            {
                if (ApplicationDateFrom > ApplicationDateTo)
                    errors.Add("ApplicationDateFrom cannot be after ApplicationDateTo");
            }

            // Validate amount range
            if (ApprovedAmountMin.HasValue && ApprovedAmountMax.HasValue)
            {
                if (ApprovedAmountMin > ApprovedAmountMax)
                    errors.Add("ApprovedAmountMin cannot be greater than ApprovedAmountMax");
            }

            // Validate sort field
            var validSortFields = new[] { "ProcessNumber", "CustomerName", "ApprovedAmount", "Status", "ApplicationDate", "Partner" };
            if (!validSortFields.Contains(SortBy))
                errors.Add($"SortBy must be one of: {string.Join(", ", validSortFields)}");

            // Validate sort order
            if (!new[] { "asc", "desc" }.Contains(SortOrder?.ToLower() ?? ""))
                errors.Add("SortOrder must be 'asc' or 'desc'");

            return errors;
        }

        /// <summary>
        /// Creates a copy of this filter with pagination reset to page 1.
        /// Useful when filter criteria change but pagination should reset.
        /// </summary>
        /// <returns>New filter instance with page number = 1</returns>
        public ApplicationFilterDto ResetPagination()
        {
            return new ApplicationFilterDto
            {
                PageNumber = 1,
                PageSize = this.PageSize,
                CustomerName = this.CustomerName,
                ProcessNumber = this.ProcessNumber,
                PartnerId = this.PartnerId,
                StatusCode = this.StatusCode,
                ApplicationDateFrom = this.ApplicationDateFrom,
                ApplicationDateTo = this.ApplicationDateTo,
                ApprovedAmountMin = this.ApprovedAmountMin,
                ApprovedAmountMax = this.ApprovedAmountMax,
                SortBy = this.SortBy,
                SortOrder = this.SortOrder
            };
        }
    }
}
