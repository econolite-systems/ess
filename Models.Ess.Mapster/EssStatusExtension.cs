// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Models.Entities;
using Econolite.Ode.Models.Ess.Db;
using Econolite.Ode.Models.Ess.Status.Db;
using Econolite.Ode.Status.Ess;

namespace Econolite.Ode.Models.Ess.Status
{
    public static partial class EssStatusExtension
    {
        public static EssStatusDto AdaptStatusDocumentToDto(this EssStatusDocument essStatusMessage, EntityNode? sensor)
        {
            return SetSensor(essStatusMessage, sensor);
        }

        public static EssStatusDto AdaptStatusDocumentWithListToDto(this EssStatusDocument essStatusMessage, IEnumerable<EntityNode> sensorCollection)
        {
            var sensor = sensorCollection.FirstOrDefault(x => x.Id == essStatusMessage.DeviceId);

            return SetSensor(essStatusMessage, sensor);
        }

        public static EssStatusDto AdaptStatusToDto(this EssStatus essStatusMessage, EntityNode? sensor)
        {
            return new EssStatusDto()
            {
                DeviceId = essStatusMessage.DeviceId,
                TimeStamp = essStatusMessage.TimeStamp,
                WetBulbTemp = essStatusMessage.WetBulbTemp,
                DewPointTemp = essStatusMessage.DewPointTemp,
                MaxTemp = essStatusMessage.MaxTemp,
                MinTemp = essStatusMessage.MinTemp,
                AdjacentSnowDepth = essStatusMessage.AdjacentSnowDepth,
                RoadwaySnowDepth = essStatusMessage.RoadwaySnowDepth,
                RoadwaySnowPackDepth = essStatusMessage.RoadwaySnowPackDepth,
                PrecipYesNo = essStatusMessage.PrecipYesNo,
                PrecipRate = essStatusMessage.PrecipRate,
                SnowfallAccumRate = essStatusMessage.SnowfallAccumRate,
                PrecipSituation = essStatusMessage.PrecipSituation,
                IceThickness = essStatusMessage.IceThickness,
                PrecipitationStartTime = essStatusMessage.PrecipitationStartTime,
                PrecipitationEndTime = essStatusMessage.PrecipitationEndTime,
                Visibility = essStatusMessage.Visibility,
                VisibilitySituation = essStatusMessage.VisibilitySituation,
                TotalSun = essStatusMessage.TotalSun,
                InstantaneousTerrestrialRadiation = essStatusMessage.InstantaneousTerrestrialRadiation,
                InstantaneousSolarRadiation = essStatusMessage.InstantaneousSolarRadiation,
                TotalRadiation = essStatusMessage.TotalRadiation,
                TotalRadiationPeriod = essStatusMessage.TotalRadiationPeriod,
                CloudSituation = essStatusMessage.CloudSituation,
                RelativeHumidity = essStatusMessage.RelativeHumidity,
                AtmosphericPressure = essStatusMessage.AtmosphericPressure,
                Latitude = sensor == null ? null : sensor.Geometry.Point?.Coordinates?[1],
                Longitude = sensor == null ? null : sensor.Geometry.Point?.Coordinates?[0],
                Name = sensor == null ? "Unknown" : sensor.Name,
            };
        }

        private static EssStatusDto SetSensor(EssStatusDocument essStatusMessage, EntityNode? sensor)
        {
            return new EssStatusDto()
            {
                DeviceId = essStatusMessage.DeviceId,
                TimeStamp = essStatusMessage.TimeStamp,
                WetBulbTemp = essStatusMessage.WetBulbTemp,
                DewPointTemp = essStatusMessage.DewPointTemp,
                MaxTemp = essStatusMessage.MaxTemp,
                MinTemp = essStatusMessage.MinTemp,
                AdjacentSnowDepth = essStatusMessage.AdjacentSnowDepth,
                RoadwaySnowDepth = essStatusMessage.RoadwaySnowDepth,
                RoadwaySnowPackDepth = essStatusMessage.RoadwaySnowPackDepth,
                PrecipYesNo = essStatusMessage.PrecipYesNo,
                PrecipRate = essStatusMessage.PrecipRate,
                SnowfallAccumRate = essStatusMessage.SnowfallAccumRate,
                PrecipSituation = essStatusMessage.PrecipSituation,
                IceThickness = essStatusMessage.IceThickness,
                PrecipitationStartTime = essStatusMessage.PrecipitationStartTime,
                PrecipitationEndTime = essStatusMessage.PrecipitationEndTime,
                Visibility = essStatusMessage.Visibility,
                VisibilitySituation = essStatusMessage.VisibilitySituation,
                TotalSun = essStatusMessage.TotalSun,
                InstantaneousTerrestrialRadiation = essStatusMessage.InstantaneousTerrestrialRadiation,
                InstantaneousSolarRadiation = essStatusMessage.InstantaneousSolarRadiation,
                TotalRadiation = essStatusMessage.TotalRadiation,
                TotalRadiationPeriod = essStatusMessage.TotalRadiationPeriod,
                CloudSituation = essStatusMessage.CloudSituation,
                RelativeHumidity = essStatusMessage.RelativeHumidity,
                AtmosphericPressure = essStatusMessage.AtmosphericPressure,
                Latitude = sensor == null ? null : sensor.Geometry.Point?.Coordinates?[1],
                Longitude = sensor == null ? null : sensor.Geometry.Point?.Coordinates?[0],
                Name = sensor == null ? "Unknown" : sensor.Name,
            };
        }
    }
}
