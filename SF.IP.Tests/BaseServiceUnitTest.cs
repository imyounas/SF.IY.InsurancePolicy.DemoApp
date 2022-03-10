using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using SF.IP.Application.Common;
using SF.IP.Domain.Entities;
using SF.IP.Infrastructure.Database;
using System;


namespace SF.IP.Tests;

public abstract class BaseServiceUnitTest : IDisposable
{
    protected ApplicationDbContext MockContext;
    protected Mock<IOptions<AppSettings>> SettingsMock;

    protected BaseServiceUnitTest()
    {
        MockContext = new ApplicationDbContext(GetOptionsBuilder());
        SettingsMock = new Mock<IOptions<AppSettings>>();
    }

    protected void SetUpPolicyUSZipCodes()
    {
        var zip1 = new USZip() { Id = 1, City = "las vegas", StateCode = "nv", StateName = "nevada", ZipCode = "89144" };
        var zip2 = new USZip() { Id = 2, City = "melbourne", StateCode = "fl", StateName = "florida", ZipCode = "32904" };
        var zip3 = new USZip() { Id = 3, City = "sioux center", StateCode = "ia", StateName = "iowa", ZipCode = "51250" };
        var zip4 = new USZip() { Id = 4, City = "minneapolis", StateCode = "mn", StateName = "minnesota", ZipCode = "55448" };

        MockContext.USZips.Add(zip1);
        MockContext.USZips.Add(zip2);
        MockContext.USZips.Add(zip3);
        MockContext.USZips.Add(zip4);

        MockContext.SaveChanges();
    }

    public void Dispose()
    {
        MockContext.Database.EnsureDeleted();
    }

    private static DbContextOptions<ApplicationDbContext> GetOptionsBuilder()
    {
        return new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }
}

