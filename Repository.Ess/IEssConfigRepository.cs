// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Models.Ess.Db;
using Econolite.Ode.Persistence.Common.Repository;

namespace Econolite.Ode.Repository.Ess;

public interface IEssConfigRepository : IRepository<EnvironmentalSensor, Guid>
{
}
