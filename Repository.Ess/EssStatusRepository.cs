// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Models.Ess.Status.Db;
using Econolite.Ode.Persistence.Mongo.Context;
using Econolite.Ode.Status.Ess;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Econolite.Ode.Repository.Ess;

public class EssStatusRepository : IEssStatusRepository
{
    private const string TimeStampFieldName = "timeStamp";
    private const string DeviceIdFieldName = "deviceId";
    private readonly IMongoCollection<EssStatusDocument> _essStatusCollection;


    public EssStatusRepository(IConfiguration configuration, IMongoContext mongoContext)
    {
        _essStatusCollection = mongoContext.GetCollection<EssStatusDocument>(configuration["Collections:EssStatus"]);
    }

    public async Task InsertAsync(EssStatus statusMessage)
    {
        await _essStatusCollection.InsertOneAsync(new EssStatusDocument(
            DeviceId: statusMessage.DeviceId,
            TimeStamp: statusMessage.TimeStamp,
            WetBulbTemp: statusMessage.WetBulbTemp,
            DewPointTemp: statusMessage.DewPointTemp,
            MaxTemp: statusMessage.MaxTemp,
            MinTemp: statusMessage.MinTemp,
            AdjacentSnowDepth: statusMessage.AdjacentSnowDepth,
            RoadwaySnowDepth: statusMessage.RoadwaySnowDepth,
            RoadwaySnowPackDepth: statusMessage.RoadwaySnowPackDepth,
            PrecipYesNo: statusMessage.PrecipYesNo,
            PrecipRate: statusMessage.PrecipRate,
            SnowfallAccumRate: statusMessage.SnowfallAccumRate,
            PrecipSituation: statusMessage.PrecipSituation,
            IceThickness: statusMessage.IceThickness,
            PrecipitationStartTime: statusMessage.PrecipitationStartTime,
            PrecipitationEndTime: statusMessage.PrecipitationEndTime,
            Visibility: statusMessage.Visibility,
            VisibilitySituation: statusMessage.VisibilitySituation,
            TotalSun: statusMessage.TotalSun,
            InstantaneousTerrestrialRadiation: statusMessage.InstantaneousTerrestrialRadiation,
            InstantaneousSolarRadiation: statusMessage.InstantaneousSolarRadiation,
            TotalRadiation: statusMessage.TotalRadiation,
            TotalRadiationPeriod: statusMessage.TotalRadiationPeriod,
            CloudSituation: statusMessage.CloudSituation,
            RelativeHumidity: statusMessage.RelativeHumidity,
            AtmosphericPressure: statusMessage.AtmosphericPressure
        ));
    }

    public async Task<EssStatusDocument?> FindLatest(Guid deviceId)
    {
        var options = new FindOptions<EssStatusDocument, EssStatusDocument>
        {
            Limit = 1,
            Sort = Builders<EssStatusDocument>.Sort.Descending(s => s.TimeStamp)
        };

        var cursor = await _essStatusCollection.FindAsync(
            s => s.DeviceId == deviceId,
            options
        );
        try
        {
            return await cursor.FirstAsync();
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    public async Task<List<EssStatusDocument>> FindAllLatest()
    {
        PipelineDefinition<EssStatusDocument, EssStatusDocument> pipeline = new[]
        {
            // Sort all environmental sensor status entries by timestamp in descending order.
            new("$sort", new BsonDocument(TimeStampFieldName, -1)),

            // Group statuses by device ID and save the first document found in each group. This will be the
            // status with the latest/largest timestamp for each device.
            new BsonDocument
            {
                {
                    "$group", new BsonDocument
                    {
                        { "_id", $"${DeviceIdFieldName}" },
                        { "mostRecentStatus", new BsonDocument("$first", "$$ROOT") }
                    }
                }
            },

            // The pipeline should output environmental sensor status objects, so replace the root group documents
            // with the most recent status documents found.
            new BsonDocument("$replaceRoot", new BsonDocument("newRoot", "$mostRecentStatus"))
        };

        var cursor = await _essStatusCollection.AggregateAsync(pipeline, new AggregateOptions());
        return await cursor.ToListAsync();
    }

    public async Task<List<EssStatusDocument>> Find(List<Guid> deviceIds, DateTime startDate, DateTime? endDate)
    {
        return await _queryStatusAsync(deviceIds, startDate, endDate);
    }

    private static FilterDefinition<EssStatusDocument> _makeDateRangeFilter(DateTime startDate, DateTime? endDate)
    {
        var afterStart = Builders<EssStatusDocument>.Filter.Gte(s => s.TimeStamp, startDate);

        if (endDate is null) return afterStart;

        var beforeEnd = Builders<EssStatusDocument>.Filter.Lte(s => s.TimeStamp, endDate);
        return afterStart & beforeEnd;
    }

    private static FilterDefinition<EssStatusDocument> _makeQueryFilter(List<Guid> deviceIds, DateTime startDate,
        DateTime? endDate)
    {
        var dateFilter = _makeDateRangeFilter(startDate, endDate);

        if (deviceIds.Count == 0)
            return dateFilter;
        return Builders<EssStatusDocument>.Filter.In(s => s.DeviceId, deviceIds) & dateFilter;
    }

    private async Task<List<EssStatusDocument>> _queryStatusAsync(List<Guid> deviceIds, DateTime startDate,
        DateTime? endDate)
    {
        var cursor = await _essStatusCollection.FindAsync(_makeQueryFilter(deviceIds, startDate, endDate));
        return await cursor.ToListAsync();
    }
}
