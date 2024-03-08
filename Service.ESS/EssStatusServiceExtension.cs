// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Microsoft.Extensions.DependencyInjection;

namespace Econolite.Ode.Services.Ess
{
    public static class EssStatusServiceExtensions
    {
        public static IServiceCollection AddEssStatusService(this IServiceCollection services)
        {
            services.AddScoped<IEssStatusService, EssStatusService>();

            return services;
        }
    }
}
