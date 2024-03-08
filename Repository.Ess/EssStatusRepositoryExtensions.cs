// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Microsoft.Extensions.DependencyInjection;

namespace Econolite.Ode.Repository.Ess;

public static class EssStatusRepositoryExtensions
{
    public static IServiceCollection AddEssStatusRepo(this IServiceCollection services)
    {
        services.AddScoped<IEssStatusRepository, EssStatusRepository>();

        return services;
    }
}
