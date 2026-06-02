using Fintech.Backoffice.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using ApplicationEntity = Fintech.Backoffice.Domain.Entities.Application;
using Customer = Fintech.Backoffice.Domain.Entities.Customer;
using Partner = Fintech.Backoffice.Domain.Entities.Partner;
using AuditLog = Fintech.Backoffice.Domain.Entities.AuditLog;

namespace Fintech.Backoffice.Infrastructure.Persistence
{
    /// <summary>
    /// Unit of Work implementation coordinating multiple repositories.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private IRepository<ApplicationEntity>? _applicationRepository;
        private IRepository<Customer>? _customerRepository;
        private IRepository<Partner>? _partnerRepository;
        private IRepository<AuditLog>? _auditLogRepository;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IRepository<ApplicationEntity> Applications =>
            _applicationRepository ??= new RepositoryBase<ApplicationEntity>(_dbContext);

        public IRepository<Customer> Customers =>
            _customerRepository ??= new RepositoryBase<Customer>(_dbContext);

        public IRepository<Partner> Partners =>
            _partnerRepository ??= new RepositoryBase<Partner>(_dbContext);

        public IRepository<AuditLog> AuditLogs =>
            _auditLogRepository ??= new RepositoryBase<AuditLog>(_dbContext);

        public async Task<int> SaveAsync()
        {
            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error saving changes to database", ex);
            }
        }

        public async Task RollbackAsync()
        {
            foreach (var entry in _dbContext.ChangeTracker.Entries().ToList())
            {
                await entry.ReloadAsync();
            }
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveAsync();
                if (_transaction != null)
                    await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                if (_transaction != null)
                    await _transaction.RollbackAsync();
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _dbContext?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
