// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Monitoring.Events;
using Econolite.Ode.Monitoring.Events.Extensions;
using Econolite.Ode.Monitoring.Metrics;
using Econolite.Ode.Repository.Ess;
using Econolite.Ode.Status.Ess;
using Econolite.Ode.Status.Ess.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Econolite.Ode.Worker.Ess;

public class EssStatusLogger : BackgroundService
{
    private readonly IEssStatusRepository _essStatusRepository;
    private readonly IEssStatusConsumer _essStatusConsumer;
    private readonly ILogger<EssStatusLogger> _logger;
    private readonly IMetricsCounter _loopCounter;
    private readonly UserEventFactory _userEventFactory;

    public EssStatusLogger(
        IEssStatusConsumer essStatusConsumer,
        IServiceProvider serviceProvider,
        UserEventFactory userEventFactory,
        IMetricsFactory metricsFactory,
        ILogger<EssStatusLogger> logger
    )
    {
        _essStatusConsumer = essStatusConsumer;
        _logger = logger;
        _userEventFactory = userEventFactory;

        _loopCounter = metricsFactory.GetMetricsCounter("Ess Status Logger");

        var serviceScope = serviceProvider.CreateScope();
        _essStatusRepository = serviceScope.ServiceProvider.GetRequiredService<IEssStatusRepository>();
    }

    private async Task LogEssStatusAsync(EssStatus status)
    {
        await _essStatusRepository.InsertAsync(status);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = _essStatusConsumer.Consume(stoppingToken);

                        try
                        {
                            await LogEssStatusAsync(result.EssStatus);
                            _essStatusConsumer.Complete(result.ConsumeResult);

                            _loopCounter.Increment();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Unhandled exception while processing EssStatus");

                            _logger.ExposeUserEvent(_userEventFactory.BuildUserEvent(EventLevel.Error, string.Format("Error while processing EssStatus: {0}", result.EssStatus?.TimeStamp)));
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Exception thrown while trying to consume DeviceStatusMessage");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Stopping");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Processing loop stopped");
            }
        }, stoppingToken);
        return Task.CompletedTask;
    }
}
