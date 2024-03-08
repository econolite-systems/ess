// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Messaging;
using Econolite.Ode.Messaging.Elements;
using Econolite.Ode.Monitoring.Events;
using Econolite.Ode.Monitoring.Events.Extensions;
using Econolite.Ode.Monitoring.Metrics;
using Econolite.Ode.Status.Common;
using Econolite.Ode.Status.Ess;
using Econolite.Ode.Status.Ess.Messaging;
using Status.Ess.Cache;
using StatusConsumer.Extensions;

namespace StatusConsumer;

public class ConsumerService : BackgroundService
{
    private readonly ILogger _logger;
    private readonly IEssStatusConsumer _essStatusConsumer;
    private readonly IEssStatusCache _essStatusCache;
    private readonly ISink<ActionEventStatus> _actionEventStatusSink;
    private readonly UserEventFactory _userEventFactory;
    private readonly IMetricsCounter _loopCounter;

    public ConsumerService(ISink<ActionEventStatus> actionEventStatusSink, IEssStatusConsumer essStatusConsumer, IEssStatusCache essStatusCache, IConfiguration configuration, UserEventFactory userEventFactory, IMetricsFactory metricsFactory, ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger(GetType().Name);
        _essStatusConsumer = essStatusConsumer;
        _essStatusCache = essStatusCache;
        _userEventFactory = userEventFactory;
        _actionEventStatusSink = actionEventStatusSink;
        _loopCounter = metricsFactory.GetMetricsCounter("Status");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(async () =>
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    (ConsumeResult<Guid, DeviceCommStatus> ConsumeResult, EssStatus EssStatus) consumeResult = default;
                    try
                    {
                        consumeResult = _essStatusConsumer.Consume(stoppingToken);
                        //To event status
                        var essActionEventStatus = EssStatusConsumerExtensions.ToEssActionEventStatus(consumeResult.EssStatus);
                        await Task.WhenAll(_essStatusCache.PutStatusAsync(consumeResult.EssStatus.DeviceId, consumeResult.EssStatus, stoppingToken), _actionEventStatusSink.SinkAsync(consumeResult.ConsumeResult.TenantId, essActionEventStatus, stoppingToken));
                        _essStatusConsumer.Complete(consumeResult.ConsumeResult);

                        _loopCounter.Increment();
                    }
                    catch (Confluent.Kafka.ConsumeException ex)
                    {
                        _logger.LogWarning(ex, "Lost connection to topic");
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unable to process result");

                        _logger.ExposeUserEvent(_userEventFactory.BuildUserEvent(EventLevel.Error, string.Format("Unable to process result: {0}, {1}", consumeResult.EssStatus?.DeviceId, consumeResult.EssStatus?.TimeStamp)));
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Stopping status processing loop");
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Status processing stopped unexpectedly, {@Exception}", e);
                throw;
            }

            _logger.LogInformation("Status processing loop stopped");
        }, stoppingToken);
    }
}
