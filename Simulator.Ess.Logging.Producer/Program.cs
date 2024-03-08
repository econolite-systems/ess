// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Common.Extensions;
using Econolite.Ode.Messaging;
using Econolite.Ode.Messaging.Extensions;
using Econolite.Ode.Monitoring.Events.Extensions;
using Econolite.Ode.Monitoring.Metrics.Extensions;
using Econolite.Ode.Persistence.Mongo;
using Econolite.Ode.Repository.Ess;
using Econolite.Ode.Simulator.Ess.Logging.Producer;
using Econolite.Ode.Status.Ess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builderContext, services) =>
    {
        services.AddTransient<IProducer<Guid, EssStatus>, Producer<Guid, EssStatus>>();

        services.AddMongo();
        services.AddMessaging()
            .AddMessagingJsonSink<EssStatus>(_ =>
            {
                _.DefaultChannel = builderContext.Configuration[Consts.TOPICS_DEVICESTATUS] ?? "devicestatus";
                _.IncludeInternalDebug = true;
            })
            .AddEssStatusRepo()
            .AddMetrics(builderContext.Configuration, "Ess Simulator Producer")
            .AddUserEventSupport(builderContext.Configuration, _ =>
            {
                _.DefaultSource = "Ess Simulator Producer";
                _.DefaultLogName = Econolite.Ode.Monitoring.Events.LogName.SystemEvent;
                _.DefaultCategory = Econolite.Ode.Monitoring.Events.Category.Server;
                _.DefaultTenantId = Guid.Empty;
            })
            .AddHostedService<EssProducer>();
    })
    .Build();

host.LogStartup();

await host.RunAsync();
