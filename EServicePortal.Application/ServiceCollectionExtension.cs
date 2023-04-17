using EServicePortal.Application.Common.Behaviours;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EServicePortal.Application;

public static class ServiceCollectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtension).Assembly, includeInternalTypes: true);
        services.AddMediatR(typeof(ServiceCollectionExtension).Assembly);
        services.AddTransient(typeof(IRequestPreProcessor<>), typeof(ValidationBehaviour<>));
    }
}
