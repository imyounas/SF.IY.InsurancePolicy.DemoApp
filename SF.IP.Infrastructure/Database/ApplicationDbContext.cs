
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SF.IP.Application.Interfaces.Database;
using SF.IP.Domain.Common;
using SF.IP.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SF.IP.Infrastructure.Database;
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<InsurancePolicy> InsurancePolicies => Set<InsurancePolicy>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<USZip> USZips => Set<USZip>();

    protected override void OnModelCreating(ModelBuilder builder)
    {

        builder
         .Entity<InsurancePolicy>()
         .HasKey(x => x.Id);

        builder
         .Entity<InsurancePolicy>()
         .Property(x => x.Id).HasDefaultValueSql("NEWID()");

        builder
          .Entity<InsurancePolicy>()
          .OwnsOne(p => p.PremiumPrice);

        builder
          .Entity<InsurancePolicy>()
          .OwnsOne(p => p.Address);

        builder
          .Entity<InsurancePolicy>()
          .Ignore(p => p.Events);

        builder
         .Entity<Vehicle>()
         .HasKey(x => x.Id);

        builder
         .Entity<Vehicle>()
         .Property(x => x.Id).HasDefaultValueSql("NEWID()");

        builder
          .Entity<Vehicle>()
          .Ignore(p => p.Events);

        base.OnModelCreating(builder);
    }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var dateTimeNow = DateTime.UtcNow;
        var appUser = "-1"; // in real application , this will be the Id/claim of user extracted from header or JWT
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = appUser;
                    entry.Entity.CreatedAt = dateTimeNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = appUser;
                    entry.Entity.LastModifiedAt = dateTimeNow;
                    break;
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }


    public override int SaveChanges()
    {
        return SaveChangesAsync().GetAwaiter().GetResult();
    }

}
