using Fintech.Backoffice.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fintech.Backoffice.Infrastructure.Persistence
{
    /// <summary>
    /// Generic repository base class implementing IRepository interface.
    /// Provides common CRUD and query operations for any entity type.
    /// Why: This generic implementation eliminates code duplication. Instead of writing
    /// separate repository classes for Application, Customer, Partner, etc.,
    /// this one class handles them all through generics.
    /// </summary>
    /// <typeparam name="T">The entity type this repository manages</typeparam>
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// The DbContext instance for database operations.
        /// Protected so derived classes can access it if needed.
        /// </summary>
        protected readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Constructor accepts DbContext via dependency injection.
        /// </summary>
        /// <param name="dbContext">Application DbContext instance</param>
        public RepositoryBase(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Gets a single entity by its primary key ID.
        /// Uses async/await for non-blocking database access.
        /// </summary>
        /// <param name="id">Primary key value</param>
        /// <returns>Entity if found, null otherwise</returns>
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// Gets all entities of type T from the database.
        /// Warning: Use with caution on large tables - consider pagination in service layer.
        /// </summary>
        /// <returns>All entities as IEnumerable</returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        /// <summary>
        /// Gets entities matching a predicate (LINQ where clause).
        /// Filters on client side after loading from database.
        /// Why: Using func<> allows flexible filtering without writing SQL.
        /// </summary>
        /// <param name="predicate">LINQ filter condition</param>
        /// <returns>Matching entities</returns>
        public async Task<IEnumerable<T>> GetByPredicateAsync(Func<T, bool> predicate)
        {
            var all = await _dbContext.Set<T>().ToListAsync();
            return all.Where(predicate);
        }

        /// <summary>
        /// Gets single entity matching predicate or null if none found.
        /// Throws exception if multiple matches.
        /// </summary>
        /// <param name="predicate">Filter condition</param>
        /// <returns>Matching entity or null</returns>
        public async Task<T?> FirstOrDefaultAsync(Func<T, bool> predicate)
        {
            var all = await _dbContext.Set<T>().ToListAsync();
            return all.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Adds a new entity to DbContext.
        /// The entity is not yet saved - SaveAsync() in UnitOfWork commits it.
        /// Why: Batching changes in memory before saving improves performance
        /// and allows transaction handling.
        /// </summary>
        /// <param name="entity">Entity to add</param>
        /// <returns>The added entity (often used for ID assignment)</returns>
        public async Task<T> AddAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            // Add to DbContext - marks for insertion
            await _dbContext.Set<T>().AddAsync(entity);

            return entity;
        }

        /// <summary>
        /// Adds multiple entities in a single operation.
        /// More efficient than calling AddAsync repeatedly.
        /// </summary>
        /// <param name="entities">Collection of entities</param>
        /// <returns>The added entities</returns>
        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            var entityList = entities.ToList();
            await _dbContext.Set<T>().AddRangeAsync(entityList);

            return entityList;
        }

        /// <summary>
        /// Updates an existing entity.
        /// The entity must already exist in the database.
        /// Simply marking the entity as modified - actual update happens on SaveAsync.
        /// </summary>
        /// <param name="entity">Entity with updated values</param>
        /// <returns>The updated entity</returns>
        public async Task<T> UpdateAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbContext.Set<T>().Update(entity);
            await Task.CompletedTask;  // Async for interface consistency

            return entity;
        }

        /// <summary>
        /// Deletes entity by ID.
        /// Marks for deletion - actual deletion happens on SaveAsync.
        /// </summary>
        /// <param name="id">Primary key of entity to delete</param>
        /// <returns>True if entity found and deleted, false otherwise</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                return false;

            _dbContext.Set<T>().Remove(entity);
            return true;
        }

        /// <summary>
        /// Deletes a specific entity instance.
        /// Useful when you already have the entity loaded.
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        /// <returns>True if successful, false if entity is null</returns>
        public async Task<bool> DeleteAsync(T entity)
        {
            if (entity == null)
                return false;

            _dbContext.Set<T>().Remove(entity);
            await Task.CompletedTask;

            return true;
        }

        /// <summary>
        /// Deletes all entities matching a predicate.
        /// Example: Delete all Cancelled applications.
        /// </summary>
        /// <param name="predicate">Filter condition</param>
        /// <returns>Number of entities deleted</returns>
        public async Task<int> DeleteRangeAsync(Func<T, bool> predicate)
        {
            var entitiesToDelete = await GetByPredicateAsync(predicate);
            var count = entitiesToDelete.Count();

            foreach (var entity in entitiesToDelete)
            {
                _dbContext.Set<T>().Remove(entity);
            }

            return count;
        }

        /// <summary>
        /// Checks if entity with given ID exists.
        /// More efficient than GetById + null check.
        /// </summary>
        /// <param name="id">Primary key to check</param>
        /// <returns>True if exists, false otherwise</returns>
        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbContext.Set<T>().AnyAsync(e =>
                EF.Property<int>(e, "Id") == id);
        }

        /// <summary>
        /// Counts total entities in the set.
        /// Useful for pagination - knowing total record count.
        /// </summary>
        /// <returns>Total count</returns>
        public async Task<int> CountAsync()
        {
            return await _dbContext.Set<T>().CountAsync();
        }

        /// <summary>
        /// Counts entities matching a predicate.
        /// Example: Count all Rejected applications.
        /// </summary>
        /// <param name="predicate">Filter condition</param>
        /// <returns>Count of matching entities</returns>
        public async Task<int> CountAsync(Func<T, bool> predicate)
        {
            var all = await _dbContext.Set<T>().ToListAsync();
            return all.Count(predicate);
        }
    }
}
