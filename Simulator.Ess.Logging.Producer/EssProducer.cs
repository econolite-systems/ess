// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Messaging;
using Econolite.Ode.Monitoring.Events;
using Econolite.Ode.Monitoring.Events.Extensions;
using Econolite.Ode.Monitoring.Metrics;
using Econolite.Ode.Status.Common;
using Econolite.Ode.Status.Ess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Econolite.Ode.Simulator.Ess.Logging.Producer
{
    public class EssProducer : IHostedService
    {
        private readonly ISink<EssStatus> _sink;
        private readonly UserEventFactory _userEventFactory;
        private readonly ILogger<EssProducer> _logger;
        private readonly string _statusTopic;
        private Guid _tenantId;
        private readonly IMetricsCounter _loopCounter;

        public EssProducer(
            ISink<EssStatus> sink,
            IConfiguration config,
            UserEventFactory userEventFactory,
            IMetricsFactory metricsFactory,
            ILogger<EssProducer> logger
        )
        {
            _sink = sink;
            _userEventFactory = userEventFactory;
            _logger = logger;

            _statusTopic = config[Consts.TOPICS_DEVICESTATUS] ?? throw new NullReferenceException($"{Consts.TOPICS_DEVICESTATUS} missing in config.");
            _tenantId = Guid.Parse(config[Consts.TENANT_ID_HEADER] ?? throw new NullReferenceException($"{Consts.TENANT_ID_HEADER} missing in config."));

            _logger.LogInformation("Subscribed topic {@}", _statusTopic);

            _loopCounter = metricsFactory.GetMetricsCounter("Simulator Producer");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("\"Producing Ess status message 1\"");

                var essStatusMessage = new EssStatus
                {
                    DeviceId = Guid.Parse("88d98575-6136-420a-82cf-c2e6547b7914"),
                    TimeStamp = DateTime.UtcNow,
                    AdjacentSnowDepth = 11,
                    AtmosphericPressure = 12,
                    CloudSituation = EssCloudSituationEnum.Clear,
                    CommStatus = CommStatus.Unknown,
                    CommSuccessRate = 15,
                    DewPointTemp = 16,
                    ExternalTag = "Thing",
                    IceThickness = 13,
                    InstantaneousSolarRadiation = 18,
                    InstantaneousTerrestrialRadiation = 19,
                    MaxTemp = 100,
                    MinTemp = 0,
                    PrecipitationEndTime = DateTime.MaxValue,
                    PrecipitationStartTime = DateTime.MinValue,
                    PrecipRate = 2,
                    PrecipSituation = EssPrecipSituationEnum.RainSlight,
                    PrecipYesNo = EssPrecipYesNoEnum.Precip,
                    RelativeHumidity = 4,
                    RoadwaySnowDepth = 5,
                    RoadwaySnowPackDepth = 3,
                    SnowfallAccumRate = 6,
                    TotalRadiation = 7,
                    TotalRadiationPeriod = 8,
                    TotalSun = 9,
                    Visibility = 10,
                    VisibilitySituation = EssVisibilitySituationEnum.VehicleSpray,
                    WetBulbTemp = 11,
                };

                await _sink.SinkAsync(_tenantId, essStatusMessage, cancellationToken);

                _logger.LogInformation("\"Producing Ess status message 2\"");
                var essStatusMessage2 = new EssStatus
                {
                    DeviceId = Guid.Parse("88d98575-6136-420a-82cf-c2e6547b7914"),
                    TimeStamp = DateTime.UtcNow,
                    AdjacentSnowDepth = 11,
                    AtmosphericPressure = 12,
                    CloudSituation = EssCloudSituationEnum.Clear,
                    CommStatus = CommStatus.Unknown,
                    CommSuccessRate = 15,
                    DewPointTemp = 16,
                    ExternalTag = "Thing",
                    IceThickness = 13,
                    InstantaneousSolarRadiation = 18,
                    InstantaneousTerrestrialRadiation = 19,
                    MaxTemp = 100,
                    MinTemp = 0,
                    PrecipitationEndTime = DateTime.MaxValue,
                    PrecipitationStartTime = DateTime.MinValue,
                    PrecipRate = 2,
                    PrecipSituation = EssPrecipSituationEnum.RainSlight,
                    PrecipYesNo = EssPrecipYesNoEnum.Precip,
                    RelativeHumidity = 4,
                    RoadwaySnowDepth = 5,
                    RoadwaySnowPackDepth = 3,
                    SnowfallAccumRate = 6,
                    TotalRadiation = 7,
                    TotalRadiationPeriod = 8,
                    TotalSun = 9,
                    Visibility = 10,
                    VisibilitySituation = EssVisibilitySituationEnum.VehicleSpray,
                    WetBulbTemp = 11,
                };

                await _sink.SinkAsync(_tenantId, essStatusMessage2, cancellationToken);

                _logger.LogInformation("\"Producing Ess status message 3\"");
                var essStatusMessage3 = new EssStatus
                {
                    DeviceId = Guid.Parse("88d98575-6136-420a-82cf-c2e6547b7914"),
                    TimeStamp = DateTime.UtcNow,
                    AdjacentSnowDepth = 11,
                    AtmosphericPressure = 12,
                    CloudSituation = EssCloudSituationEnum.Clear,
                    CommStatus = CommStatus.Unknown,
                    CommSuccessRate = 15,
                    DewPointTemp = 16,
                    ExternalTag = "Thing",
                    IceThickness = 13,
                    InstantaneousSolarRadiation = 18,
                    InstantaneousTerrestrialRadiation = 19,
                    MaxTemp = 100,
                    MinTemp = 0,
                    PrecipitationEndTime = DateTime.MaxValue,
                    PrecipitationStartTime = DateTime.MinValue,
                    PrecipRate = 2,
                    PrecipSituation = EssPrecipSituationEnum.RainSlight,
                    PrecipYesNo = EssPrecipYesNoEnum.Precip,
                    RelativeHumidity = 4,
                    RoadwaySnowDepth = 5,
                    RoadwaySnowPackDepth = 3,
                    SnowfallAccumRate = 6,
                    TotalRadiation = 7,
                    TotalRadiationPeriod = 8,
                    TotalSun = 9,
                    Visibility = 10,
                    VisibilitySituation = EssVisibilitySituationEnum.VehicleSpray,
                    WetBulbTemp = 11,
                };

                await _sink.SinkAsync(_tenantId, essStatusMessage3, cancellationToken);

                _logger.LogInformation("\"Producing Ess status message 4\"");
                var essStatusMessage4 = new EssStatus
                {
                    DeviceId = Guid.Parse("88d98575-6136-420a-82cf-c2e6547b7914"),
                    TimeStamp = DateTime.UtcNow,
                    AdjacentSnowDepth = 11,
                    AtmosphericPressure = 12,
                    CloudSituation = EssCloudSituationEnum.Clear,
                    CommStatus = CommStatus.Unknown,
                    CommSuccessRate = 15,
                    DewPointTemp = 16,
                    ExternalTag = "Thing",
                    IceThickness = 13,
                    InstantaneousSolarRadiation = 18,
                    InstantaneousTerrestrialRadiation = 19,
                    MaxTemp = 100,
                    MinTemp = 0,
                    PrecipitationEndTime = DateTime.MaxValue,
                    PrecipitationStartTime = DateTime.MinValue,
                    PrecipRate = 2,
                    PrecipSituation = EssPrecipSituationEnum.RainSlight,
                    PrecipYesNo = EssPrecipYesNoEnum.Precip,
                    RelativeHumidity = 4,
                    RoadwaySnowDepth = 5,
                    RoadwaySnowPackDepth = 3,
                    SnowfallAccumRate = 6,
                    TotalRadiation = 7,
                    TotalRadiationPeriod = 8,
                    TotalSun = 9,
                    Visibility = 10,
                    VisibilitySituation = EssVisibilitySituationEnum.VehicleSpray,
                    WetBulbTemp = 11,
                };

                await _sink.SinkAsync(_tenantId, essStatusMessage4, cancellationToken);

                _loopCounter.Increment(4);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to produce ESS messages");

                _logger.ExposeUserEvent(_userEventFactory.BuildUserEvent(EventLevel.Error, "Unable to produce ESS messages"));

                throw;
            }
            finally
            {
                _logger.LogInformation("\"Finished producing Ess status messages\"");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
