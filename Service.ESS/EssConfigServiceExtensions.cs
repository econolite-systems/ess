// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Microsoft.Extensions.DependencyInjection;

namespace Econolite.Ode.Services.Ess
{
    public static class EssConfigServiceExtensions
    {
        public static IServiceCollection AddEssConfigService(this IServiceCollection services)
        {
            services.AddScoped<IEssConfigService, EssConfigService>();

            return services;
        }
    }
}
