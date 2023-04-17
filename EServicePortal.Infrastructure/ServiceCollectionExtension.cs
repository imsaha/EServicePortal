using EServicePortal.Application.Common.Interfaces;
using EServicePortal.Infrastructure.Persistence;
using EServicePortal.Infrastructure.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EServicePortal.Infrastructure;

public static class ServiceCollectionExtension
{

    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAppDbContext>(x => x.GetService<AppDbContext>()!);

        services
            .AddDataProtection()
            .SetApplicationName("E_Service_Portal")
            .PersistKeysToDbContext<AppDbContext>();

        services.AddTransient<ITokenService, TokenService>();
    }
}
