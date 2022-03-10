using CsvHelper;
using SF.IP.Domain.Entities;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SF.IP.Infrastructure.Database;

public static class ApplicationDbSeed
{
    public static async Task SeedSampleDataAsync(ApplicationDbContext dbContext)
    {
        // Seed, if necessary
        if (!dbContext.USZips.Any())
        {
            List<USZip> usZips = new List<USZip>();
            using (var reader = new StreamReader(@"uszips.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                usZips = csv.GetRecords<USZip>().ToList();
            }

            foreach (var zip in usZips)
            {
                dbContext.USZips.Add(new USZip() { City = zip.City.ToLower(), StateCode = zip.StateCode.ToLower(), StateName = zip.StateName.ToLower(), ZipCode = zip.ZipCode.ToLower() });
            }

            await dbContext.SaveChangesAsync();
        }
    }
}

