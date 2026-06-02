using Fintech.Backoffice.Domain.Entities;

namespace Fintech.Backoffice.Domain.Interfaces
{
    /// <summary>
    /// Unit of Work pattern interface.
    /// Centralizes all repository instances and manages transactions.
    /// Why: Unit of Work ensures that multiple repository operations either all succeed or all fail together.
    /// This maintains data consistency - imagine updating an Application and AuditLog:
    /// if the audit fails but application succeeds, data becomes inconsistent.
    /// UnitOfWork prevents this by wrapping changes in a single transaction.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Repository for Application entity operations.
        /// </summary>
        IRepository<Application> Applications { get; }

        /// <summary>
        /// Repository for Customer entity operations.
        /// </summary>
        IRepository<Customer> Customers { get; }

        /// <summary>
        /// Repository for Partner entity operations.
        /// </summary>
        IRepository<Partner> Partners { get; }

        /// <summary>
        /// Repository for AuditLog entity operations.
        /// </summary>
        IRepository<AuditLog> AuditLogs { get; }

        /// <summary>
        /// Saves all changes made through repositories in this unit of work.
        /// This is where database writes actually happen - all previous Add/Update/Delete
        /// operations were only staged in memory.
        /// Why: Batching changes allows efficient bulk operations and atomic transactions.
        /// </summary>
        /// <returns>Number of records affected</returns>
        Task<int> SaveAsync();

        /// <summary>
        /// Rolls back all pending changes without saving.
        /// Useful if validation fails after changes are staged.
        /// Why: Allows "prepare-check-commit" pattern for complex operations.
        /// </summary>
        Task RollbackAsync();

        /// <summary>
        /// Begins a database transaction.
        /// Multiple SaveAsync calls within the same transaction either all commit or all rollback.
        /// Advanced usage - usually SaveAsync handles transactions automatically.
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commits the current transaction.
        /// Use after BeginTransactionAsync for advanced transaction control.
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Rolls back the current transaction.
        /// Reverts all changes made in the transaction.
        /// </summary>
        Task RollbackTransactionAsync();
    }
}
