using Econolite.Ode.Messaging.Elements;
using Econolite.Ode.Status.Common;
using Econolite.Ode.Status.Ess;

namespace StatusConsumer.Extensions
{
    public static class EssStatusConsumerExtensions
    {
        public static EssActionEventStatus ToEssActionEventStatus(EssStatus essStatus)
        {
            return new EssActionEventStatus()
            {
                ActionEventType = "EnvironmentalSensor",
                AtmosphericPressure = essStatus.AtmosphericPressure,
                AdjacentSnowDepth = essStatus.AdjacentSnowDepth,
                CloudSituation = essStatus.CloudSituation,
                DeviceId = essStatus.DeviceId,
                DewPointTemp = essStatus.DewPointTemp,
                IceThickness = essStatus.IceThickness,
                InstantaneousSolarRadiation = essStatus.InstantaneousSolarRadiation,
                InstantaneousTerrestrialRadiation = essStatus.InstantaneousTerrestrialRadiation,
                MaxTemp = essStatus.MaxTemp,
                MinTemp = essStatus.MinTemp,
                PassiveRoadSensorEntries = essStatus.PassiveRoadSensorEntries,
                PrecipitationEndTime = essStatus.PrecipitationEndTime,
                PrecipitationStartTime = essStatus.PrecipitationStartTime,
                PrecipRate = essStatus.PrecipRate,
                PrecipSituation = essStatus.PrecipSituation,
                PrecipYesNo = essStatus.PrecipYesNo,
                RelativeHumidity = essStatus.RelativeHumidity,
                RoadwaySnowDepth = essStatus.RoadwaySnowDepth,
                RoadwaySnowPackDepth = essStatus.RoadwaySnowPackDepth,
                SnowfallAccumRate = essStatus.SnowfallAccumRate,
                TimeStamp = essStatus.TimeStamp,
                TotalRadiation = essStatus.TotalRadiation,
                TotalRadiationPeriod = essStatus.TotalRadiationPeriod,
                TotalSun = essStatus.TotalSun,
                Visibility = essStatus.Visibility,
                VisibilitySituation = essStatus.VisibilitySituation,
                WetBulbTemp = essStatus.WetBulbTemp

            };
        }
    }
}
