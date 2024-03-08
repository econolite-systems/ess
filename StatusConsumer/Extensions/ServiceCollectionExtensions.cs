// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Status.Common.Messaging.Extensions;
using Econolite.Ode.Status.Ess.Messaging.Extensions;

namespace StatusConsumer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommonServices(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            services.AddActionEventStatusSink(configuration);
            services.AddEssStatusSink(configuration);

            return services;
        }
    }
}
