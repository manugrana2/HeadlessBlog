using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HeadLessBlog.Infrastructure.Persistence;

public class HeadLessBlogDbContextFactory : IDesignTimeDbContextFactory<HeadLessBlogDbContext>
{
    public HeadLessBlogDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../HeadLessBlog.WebAPI");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.Development.json", optional: false)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<HeadLessBlogDbContext>();
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

        return new HeadLessBlogDbContext(optionsBuilder.Options);
    }
}
