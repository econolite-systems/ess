// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Models.Ess.Dto;

namespace Econolite.Ode.Services.Ess;

public interface IEssConfigService
{
    Task<IEnumerable<EnvironmentalSensorDto>> GetAllAsync();
    Task<EnvironmentalSensorDto?> GetByIdAsync(Guid id);
    Task<EnvironmentalSensorDto?> Add(EnvironmentalSensorAdd add);
    Task<EnvironmentalSensorDto?> Update(EnvironmentalSensorUpdate update);
    Task<bool> Delete(Guid id);
}
