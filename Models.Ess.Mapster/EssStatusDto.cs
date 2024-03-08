// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Status.Ess;

namespace Econolite.Ode.Models.Ess.Status
{
    public partial record EssStatusDto
    {
    public Guid DeviceId { get; set; }
    public DateTime TimeStamp { get; set; }
    public int WetBulbTemp { get; set; }
    public int DewPointTemp { get; set; }
    public int MaxTemp { get; set; }
    public int MinTemp { get; set; }
    public int AdjacentSnowDepth { get; set; }
    public int RoadwaySnowDepth { get; set; }
    public int RoadwaySnowPackDepth { get; set; }
    public EssPrecipYesNoEnum PrecipYesNo { get; set; }
    public int PrecipRate { get; set; }
    public int SnowfallAccumRate { get; set; }
    public EssPrecipSituationEnum PrecipSituation { get; set; }
    public int IceThickness { get; set; }
    public DateTime PrecipitationStartTime { get; set; }
    public DateTime PrecipitationEndTime { get; set; }
    public int Visibility { get; set; }
    public EssVisibilitySituationEnum VisibilitySituation { get; set; }
    public int TotalSun { get; set; }
    public int InstantaneousTerrestrialRadiation { get; set; }
    public int InstantaneousSolarRadiation { get; set; }
    public int TotalRadiation { get; set; }
    public int TotalRadiationPeriod { get; set; }
    public EssCloudSituationEnum CloudSituation { get; set; }
    public int RelativeHumidity { get; set; }
    public int AtmosphericPressure { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? Name { get; set; }
    }
}
