namespace Fintech.Backoffice.Domain.Interfaces
{
    /// <summary>
    /// Generic repository interface defining common data access operations.
    /// Why: The Repository Pattern provides an abstraction over data access,
    /// making it easy to switch databases or implement caching without changing business logic.
    /// All data access must go through this interface for consistency and testability.
    /// Generic T allows reusing this interface for all entity types.
    /// </summary>
    /// <typeparam name="T">The entity type this repository handles</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Gets an entity by its primary key ID.
        /// Returns null if entity is not found.
        /// Why: Async allows non-blocking database calls, improving application responsiveness.
        /// </summary>
        /// <param name="id">The primary key value</param>
        /// <returns>The entity if found, null otherwise</returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Gets all entities of type T.
        /// Use with caution on large tables - consider pagination in business logic layer.
        /// Returns empty list if no entities exist.
        /// </summary>
        /// <returns>List of all entities</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Gets all entities that match a predicate (where clause).
        /// Useful for filtering without fetching all records.
        /// Why: Allows database-side filtering for better performance.
        /// </summary>
        /// <param name="predicate">LINQ predicate to filter entities</param>
        /// <returns>List of matching entities</returns>
        Task<IEnumerable<T>> GetByPredicateAsync(Func<T, bool> predicate);

        /// <summary>
        /// Gets a single entity that matches a predicate.
        /// Returns null if no match found.
        /// Throws exception if multiple matches found.
        /// </summary>
        /// <param name="predicate">LINQ predicate to find single entity</param>
        /// <returns>Matching entity or null</returns>
        Task<T?> FirstOrDefaultAsync(Func<T, bool> predicate);

        /// <summary>
        /// Adds a new entity to the database.
        /// The actual save happens when UnitOfWork.SaveAsync() is called.
        /// Why: Batching changes in UnitOfWork allows transactions for consistency.
        /// </summary>
        /// <param name="entity">The entity to add</param>
        /// <returns>The added entity</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Adds multiple entities to the database in a single operation.
        /// More efficient than calling AddAsync repeatedly.
        /// </summary>
        /// <param name="entities">Collection of entities to add</param>
        /// <returns>Collection of added entities</returns>
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Updates an existing entity.
        /// The entity must already exist in the database.
        /// Why: Using this method ensures the entity is tracked for changes.
        /// </summary>
        /// <param name="entity">The entity with updated values</param>
        /// <returns>The updated entity</returns>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Deletes an entity by ID.
        /// Actually marks for deletion; real deletion happens on SaveAsync().
        /// </summary>
        /// <param name="id">Primary key of entity to delete</param>
        /// <returns>True if deletion was successful</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Deletes a specific entity instance.
        /// Useful when you already have the entity loaded.
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        /// <returns>True if deletion was successful</returns>
        Task<bool> DeleteAsync(T entity);

        /// <summary>
        /// Deletes multiple entities matching a predicate.
        /// Example: Delete all applications with status Cancelled.
        /// </summary>
        /// <param name="predicate">LINQ predicate identifying entities to delete</param>
        /// <returns>Number of entities deleted</returns>
        Task<int> DeleteRangeAsync(Func<T, bool> predicate);

        /// <summary>
        /// Checks if an entity with given ID exists.
        /// More efficient than GetById followed by null check.
        /// </summary>
        /// <param name="id">Primary key to check</param>
        /// <returns>True if entity exists, false otherwise</returns>
        Task<bool> ExistsAsync(int id);

        /// <summary>
        /// Counts total number of entities.
        /// Useful for pagination - get total record count.
        /// </summary>
        /// <returns>Total count of entities</returns>
        Task<int> CountAsync();

        /// <summary>
        /// Counts entities matching a predicate.
        /// Example: Count applications with status Rejected.
        /// </summary>
        /// <param name="predicate">LINQ predicate for filtering</param>
        /// <returns>Count of matching entities</returns>
        Task<int> CountAsync(Func<T, bool> predicate);
    }
}
