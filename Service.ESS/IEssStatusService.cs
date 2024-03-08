// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Models.Ess.Status;
using Econolite.Ode.Status.Ess;

namespace Econolite.Ode.Services.Ess;

public interface IEssStatusService
{
    Task<IEnumerable<EssStatusDto>> Find(List<Guid> deviceId, DateTime startDate, DateTime? endDate);
    Task<EssStatusDto?> FindLatest(Guid deviceId);
    Task<IEnumerable<EssStatusDto>> FindAllLatest();
    Task<EssStatusDto> Add(EssStatus status);
}
