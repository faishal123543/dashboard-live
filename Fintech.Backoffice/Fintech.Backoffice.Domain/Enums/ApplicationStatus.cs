namespace Fintech.Backoffice.Domain.Enums
{
    /// <summary>
    /// Enumeration for application status values.
    /// Represents the various states an application can have throughout its lifecycle.
    /// Why: Using enums instead of strings ensures type safety and prevents invalid status values.
    /// The int values map to the database for proper storage and querying.
    /// </summary>
    public enum ApplicationStatus
    {
        /// <summary>
        /// Application is currently being processed.
        /// </summary>
        InProgress = 1,

        /// <summary>
        /// Application has been rejected and processing stopped.
        /// </summary>
        Rejected = 2,

        /// <summary>
        /// Application has been completed successfully.
        /// </summary>
        Completed = 3,

        /// <summary>
        /// Application processing has been cancelled.
        /// </summary>
        Cancelled = 4
    }
}
