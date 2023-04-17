using EServicePortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EServicePortal.PostgresPersistence;

public static class ServiceCollectionExtension
{
    public static void AddPostgresPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options
                .UseNpgsql(configuration.GetConnectionString("db")!, builder =>
                {
                    builder.MigrationsAssembly(typeof(AppDbContextDesignTimeFactory).Assembly.FullName);
                    builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                })
                .UseSnakeCaseNamingConvention();
        });
    }
}
