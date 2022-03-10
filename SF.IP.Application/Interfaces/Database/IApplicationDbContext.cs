using Microsoft.EntityFrameworkCore;
using SF.IP.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SF.IP.Application.Interfaces.Database;

public interface IApplicationDbContext
{

    DbSet<InsurancePolicy> InsurancePolicies { get; }

    DbSet<Vehicle> Vehicles { get; }
    DbSet<USZip> USZips { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

