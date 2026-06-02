using Fintech.Backoffice.Application.DTOs;

namespace Fintech.Backoffice.Application.ViewModels
{
    /// <summary>
    /// ViewModel for Applications list view with pagination and filtering.
    /// Contains paginated list of applications and filter options.
    /// </summary>
    public class ApplicationListViewModel
    {
        /// <summary>
        /// Paginated result of applications matching the current filter.
        /// </summary>
        public PagedResultDto<ApplicationDto> Applications { get; set; } = new();

        /// <summary>
        /// Current filter criteria being applied.
        /// Allows view to display current filters and enable modification.
        /// </summary>
        public ApplicationFilterDto CurrentFilter { get; set; } = new();

        /// <summary>
        /// List of all available partners for filter dropdown.
        /// </summary>
        public List<PartnerDto> AvailablePartners { get; set; } = new();

        /// <summary>
        /// List of available status values for filter dropdown.
        /// Dictionary: Key=StatusCode (int), Value=StatusName (string)
        /// </summary>
        public Dictionary<int, string> AvailableStatuses { get; set; } = new()
        {
            { 1, "In Progress" },
            { 2, "Rejected" },
            { 3, "Completed" },
            { 4, "Cancelled" }
        };

        /// <summary>
        /// Number of applications loaded on current page.
        /// Useful for "Showing X of Y records" message.
        /// </summary>
        public int RecordsOnThisPage => Applications.Items.Count();

        /// <summary>
        /// Start record number on this page (1-based).
        /// Example: Page 2 with 10 items per page = 11
        /// </summary>
        public int StartRecordNumber =>
            (Applications.PageNumber - 1) * Applications.PageSize + 1;

        /// <summary>
        /// End record number on this page.
        /// Example: Page 1 with 10 items per page = 10
        /// </summary>
        public int EndRecordNumber =>
            Math.Min(Applications.PageNumber * Applications.PageSize, Applications.TotalCount);
    }
}
