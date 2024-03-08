// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Domain.Configuration;
using Econolite.Ode.Models.Entities;
using Econolite.Ode.Models.Ess.Status;
using Econolite.Ode.Repository.Ess;
using Microsoft.Extensions.Logging;
using Econolite.Ode.Status.Ess;

namespace Econolite.Ode.Services.Ess;

public class EssStatusService : IEssStatusService
{
    private readonly IEssStatusRepository _essStatusRepo;
    private readonly IEntityService _entityService;
    private readonly ILogger<EssStatusService> _logger;

    public EssStatusService(IEntityService entityService, IEssStatusRepository essStatusRepo, ILogger<EssStatusService> logger)
    {
        _essStatusRepo = essStatusRepo;
        _entityService = entityService;
        _logger = logger;
    }

    public async Task<EssStatusDto> Add(EssStatus status)
    {
        var nameAndLocation = await FindNameAndLocation(status.DeviceId);
        await _essStatusRepo.InsertAsync(status);
        return status.AdaptStatusToDto(nameAndLocation);
    }

    public async Task<IEnumerable<EssStatusDto>> Find(List<Guid> deviceId, DateTime startDate, DateTime? endDate)
    {
        var namesAndLocations = await FindNamesAndLocations();
        var result = await _essStatusRepo.Find(deviceId, startDate, endDate);
        return result.Select(r => r.AdaptStatusDocumentWithListToDto(namesAndLocations));
    }

    public async Task<IEnumerable<EssStatusDto>> FindAllLatest()
    {
        var namesAndLocations = await FindNamesAndLocations();
        var result = await _essStatusRepo.FindAllLatest();
        return result.Select(r => r.AdaptStatusDocumentWithListToDto(namesAndLocations));
    }

    public async Task<EssStatusDto?> FindLatest(Guid deviceId)
    {
        var nameAndLocation = await FindNameAndLocation(deviceId);
        var result = await _essStatusRepo.FindLatest(deviceId);
        return result?.AdaptStatusDocumentToDto(nameAndLocation);
    }

    private async Task<EntityNode?> FindNameAndLocation(Guid deviceId)
    {
        var sensor = await _entityService.GetByIdAsync(deviceId);
        return sensor;
    }

    private async Task<IEnumerable<EntityNode>> FindNamesAndLocations()
    {
        var sensor = await _entityService.GetNodesByTypeAsync("Environmental Sensor");
        return sensor;
    }
}
