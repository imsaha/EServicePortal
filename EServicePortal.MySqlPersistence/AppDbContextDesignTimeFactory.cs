using EServicePortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EServicePortal.MySqlPersistence;

public class AppDbContextDesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var dbContextBuilder = new DbContextOptionsBuilder<AppDbContext>();
        const string connectionString = "Host=localhost;Database=e_service_portal_db;Username=postgres;Password=root";
        dbContextBuilder.UseMySQL(connectionString, builder =>
        {
            builder.MigrationsAssembly(typeof(AppDbContextDesignTimeFactory).Assembly.FullName);
        }).UseSnakeCaseNamingConvention();
        return new AppDbContext(dbContextBuilder.Options);
    }
}
