// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Extensions.AspNet;
using Econolite.Ode.Monitoring.HealthChecks.Redis.Extensions;
using Econolite.Ode.Status.Ess.Messaging.Extensions;
using Microsoft.Extensions.Configuration;
using Status.Ess.Cache.Extensions;
using StatusConsumer;
using StatusConsumer.Extensions;


await AppBuilder.BuildAndRunWebHostAsync(args, options => { options.Source = "Ess Status Consumer"; }, (builder, services) =>
{
    builder.Services.AddCommonServices(builder.Configuration);
    services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration[Consts.RedisConnection];
        })
        .AddEssCache()
        .AddEssStatusConsumer(_ => _.ConfigTopic = Econolite.Ode.Messaging.Consts.TOPICS_DEVICESTATUS)
        .AddHostedService<ConsumerService>();
}, (builder, checksBuilder) => checksBuilder.AddRedisHealthCheck(builder.Configuration[Consts.RedisConnection] ?? throw new NullReferenceException($"{Consts.RedisConnection} missing from config.")));
