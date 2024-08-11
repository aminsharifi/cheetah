﻿using Cheetah.Infrastructure.Data;

namespace Cheetah.Sample.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
      this IServiceCollection services,
      ConfigurationManager config,
      Microsoft.Extensions.Logging.ILogger logger)
    {
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));

        return services;
    }
}
