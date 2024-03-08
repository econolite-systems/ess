// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Microsoft.Extensions.DependencyInjection;

namespace Econolite.Ode.Repository.Ess;

public static class EssConfigRepositoryExtensions
{
    public static IServiceCollection AddEssConfigRepo(this IServiceCollection services)
    {
        services.AddScoped<IEssConfigRepository, EssConfigRepository>();

        return services;
    }
}
