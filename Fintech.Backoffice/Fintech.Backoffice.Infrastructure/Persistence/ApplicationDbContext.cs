using Microsoft.EntityFrameworkCore;
using ApplicationEntity = Fintech.Backoffice.Domain.Entities.Application;
using Customer = Fintech.Backoffice.Domain.Entities.Customer;
using Partner = Fintech.Backoffice.Domain.Entities.Partner;
using AuditLog = Fintech.Backoffice.Domain.Entities.AuditLog;
using BaseEntity = Fintech.Backoffice.Domain.Common.BaseEntity;

namespace Fintech.Backoffice.Infrastructure.Persistence
{
    /// <summary>
    /// Entity Framework Core DbContext for the Fintech Backoffice application.
    /// Gateway to the database - all database operations go through this context.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApplicationEntity> Applications { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Partner> Partners { get; set; } = null!;
        public virtual DbSet<AuditLog> AuditLogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Application entity
            modelBuilder.Entity<ApplicationEntity>(entity =>
            {
                entity.ToTable("Applications", schema: "dbo");
                entity.HasKey(e => e.Id).HasName("PK_Applications");

                entity.Property(e => e.ProcessNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ApprovedAmount).HasPrecision(18, 2);
                entity.Property(e => e.Remarks).HasMaxLength(2000);

                entity.HasIndex(e => e.ProcessNumber).IsUnique();
                entity.HasIndex(e => e.ApplicationDate);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => new { e.Status, e.ApplicationDate });
                entity.HasIndex(e => e.PartnerId);
                entity.HasIndex(e => e.CustomerId);

                entity.HasOne(e => e.Customer)
                    .WithMany(c => c.Applications)
                    .HasForeignKey(e => e.CustomerId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Partner)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(e => e.PartnerId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Configure Customer entity
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers", schema: "dbo");
                entity.HasKey(e => e.Id).HasName("PK_Customers");

                entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.MobileNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.EmailAddress).HasMaxLength(255);

                entity.HasIndex(e => e.CustomerName);
                entity.HasIndex(e => e.MobileNumber);
            });

            // Configure Partner entity
            modelBuilder.Entity<Partner>(entity =>
            {
                entity.ToTable("Partners", schema: "dbo");
                entity.HasKey(e => e.Id).HasName("PK_Partners");

                entity.Property(e => e.PartnerName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.PartnerCode).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ContactEmail).HasMaxLength(255);
                entity.Property(e => e.ContactPhone).HasMaxLength(20);

                entity.HasIndex(e => e.PartnerCode).IsUnique();
                entity.HasIndex(e => e.PartnerName);
            });

            // Configure AuditLog entity
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.ToTable("AuditLogs", schema: "dbo");
                entity.HasKey(e => e.Id).HasName("PK_AuditLogs");

                entity.Property(e => e.EntityName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Action).IsRequired().HasMaxLength(50);
                entity.Property(e => e.OldValues).HasMaxLength(4000);
                entity.Property(e => e.NewValues).HasMaxLength(4000);
                entity.Property(e => e.ChangedBy).HasMaxLength(255);
                entity.Property(e => e.IpAddress).HasMaxLength(45);
                entity.Property(e => e.Reason).HasMaxLength(500);

                entity.HasIndex(e => new { e.EntityName, e.EntityId });
                entity.HasIndex(e => e.CreatedDate);
            });
        }

        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditFields()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity);

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = DateTime.UtcNow;
                    entity.ModifiedDate = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.ModifiedDate = DateTime.UtcNow;
                }
            }
        }
    }
}
