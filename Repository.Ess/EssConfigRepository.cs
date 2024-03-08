// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Models.Ess.Db;
using Econolite.Ode.Persistence.Mongo.Context;
using Econolite.Ode.Persistence.Mongo.Repository;
using Microsoft.Extensions.Logging;

namespace Econolite.Ode.Repository.Ess;

public class EssConfigRepository : GuidDocumentRepositoryBase<EnvironmentalSensor>, IEssConfigRepository
{
    public EssConfigRepository(IMongoContext context, ILogger<EssConfigRepository> logger) : base(context, logger)
    {
    }
}
