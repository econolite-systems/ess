// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Status.Ess;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Econolite.Ode.Models.Ess.Status.Db;

public sealed record EssStatusDocument(
    Guid DeviceId,
    DateTime TimeStamp,
    int WetBulbTemp,
    int DewPointTemp,
    int MaxTemp,
    int MinTemp,
    int AdjacentSnowDepth,
    int RoadwaySnowDepth,
    int RoadwaySnowPackDepth,
    EssPrecipYesNoEnum PrecipYesNo,
    int PrecipRate,
    int SnowfallAccumRate,
    EssPrecipSituationEnum PrecipSituation,
    int IceThickness,
    DateTime PrecipitationStartTime,
    DateTime PrecipitationEndTime,
    int Visibility,
    EssVisibilitySituationEnum VisibilitySituation,
    int TotalSun,
    int InstantaneousTerrestrialRadiation,
    int InstantaneousSolarRadiation,
    int TotalRadiation,
    int TotalRadiationPeriod,
    EssCloudSituationEnum CloudSituation,
    int RelativeHumidity,
    int AtmosphericPressure
)
{
    [BsonId(IdGenerator = typeof(GuidGenerator))]
    public Guid Id { get; init; }
}
