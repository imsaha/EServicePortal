using EServicePortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EServicePortal.MySqlPersistence;

public static class ServiceCollectionExtension
{
    public static void AddMySqlPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options
                .UseMySQL(configuration.GetConnectionString("db")!, builder =>
                {
                    builder.MigrationsAssembly(typeof(AppDbContextDesignTimeFactory).Assembly.FullName);
                    builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                })
                .UseSnakeCaseNamingConvention();
        });
    }
}
