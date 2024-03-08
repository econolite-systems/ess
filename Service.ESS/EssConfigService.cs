// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Models.Entities;
using Econolite.Ode.Models.Ess.Dto;
using Econolite.Ode.Repository.Ess;
using Mapster;
using Microsoft.Extensions.Logging;
using Econolite.Ode.Domain.Configuration;
using Econolite.Ode.Helpers.Exceptions;

namespace Econolite.Ode.Services.Ess;

public class EssConfigService : IEssConfigService
{
    private readonly IDeviceManagerService _deviceManagerService;
    private readonly IEntityService _entityService;
    private readonly IEssConfigRepository _essRepository;
    private readonly ILogger<EssConfigService> _logger;

    public EssConfigService(IEssConfigRepository essRepository, IEntityService entityService,
        IDeviceManagerService deviceManagerService, ILogger<EssConfigService> logger)
    {
        _deviceManagerService = deviceManagerService;
        _essRepository = essRepository;
        _entityService = entityService;
        _logger = logger;
    }

    public async Task<IEnumerable<EnvironmentalSensorDto>> GetAllAsync()
    {
        var list = await _essRepository.GetAllAsync();
        var removeDeleted = list.Where(d => !d.IsDeleted);
        return removeDeleted.Select(e => e.AdaptToDto());
    }

    public async Task<EnvironmentalSensorDto?> GetByIdAsync(Guid id)
    {
        var result = await _essRepository.GetByIdAsync(id);
        if (result is null || result.IsDeleted)
            return null;
        return result.AdaptToDto();
    }

    public async Task<EnvironmentalSensorDto?> Add(EnvironmentalSensorAdd add)
    {
        var ess = add.AdaptToEnvironmentalSensor();
        ess.Id = Guid.NewGuid();

        var entityNode = await _entityService.Add(Guid.Empty, ess.ToEntityNode(Guid.Empty));
        if (entityNode == null) return null;

        //set the controller id and name to the entity
        if (ess.Controller != null)
        {
            ess.Controller.Id = ess.Id;
            ess.Controller.Name = ess.Name;
        }
        _essRepository.Add(ess);

        var (success, _) = await _essRepository.DbContext.SaveChangesAsync();
        if (success && ess.DeviceManager != Guid.Empty && ess.Channel != Guid.Empty && ess.Controller is not null)
            await _deviceManagerService.AddControllerAsync(ess.DeviceManager, ess.Channel, ess.Controller);

        return ess.AdaptToDto();
    }

    public async Task<EnvironmentalSensorDto?> Update(EnvironmentalSensorUpdate update)
    {
        try
        {
            var ess = await _essRepository.GetByIdAsync(update.Id);
            if (ess is null)
            {
                return null;
            }
            
            var updated = update.AdaptTo(ess);

            //set the controller id and name to the entity
            if (updated.Controller != null)
            {
                updated.Controller.Id = updated.Id;
                updated.Controller.Name = updated.Name;
            }

            if ((ess.DeviceManager != updated.DeviceManager ||
                 ess.Channel != updated.Channel) && ess.DeviceManager != Guid.Empty && ess.Channel != Guid.Empty)
            {
                var deleted = await _deviceManagerService.DeleteControllerAsync(ess.DeviceManager, ess.Channel, ess.Id);
            }

            if (!ess.IsDeleted && ess.DeviceManager != Guid.Empty && ess.Channel != Guid.Empty && updated.Controller is not null)
            {
                var controller =
                    await _deviceManagerService.UpdateControllerAsync(ess.DeviceManager, ess.Channel,
                        updated.Controller);
            }

            if (!ess.IsDeleted)
            {
                var entityNode = false;
                var node = await _entityService.GetByIdAsync(update.Id);
                if (node != null)
                {
                    node.Description = ess.Description;
                    node.Name = ess.Name;
                    node.Jurisdiction = ess.Jurisdiction;
                    node.Type = ess.Type;

                    entityNode = await _entityService.Update(ess.ToEntityNode(Guid.Empty));
                }
                else
                {
                    var added = await _entityService.Add(Guid.Empty, ess.ToEntityNode(Guid.Empty));
                    entityNode = added != null;
                }

                if (!entityNode) return null;
            }

            _essRepository.Update(updated);
            var (success, errors) = await _essRepository.DbContext.SaveChangesAsync();
            if (!success && !string.IsNullOrWhiteSpace(errors)) throw new UpdateException(errors);
            return updated.AdaptToDto();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }

    public async Task<bool> Delete(Guid id)
    {
        try
        {
            var ess = await _essRepository.GetByIdAsync(id);
            if (ess != null && ess.DeviceManager != Guid.Empty && ess.Channel != Guid.Empty)
            {
                var deletedController =
                    await _deviceManagerService.DeleteControllerAsync(ess.DeviceManager, ess.Channel, ess.Id);
            }

            var dto = ess.AdaptToDto();
            dto.IsDeleted = true;
            var entityResult = _entityService.Delete(dto.Id);
            var result = await Update(dto.Adapt<EnvironmentalSensorUpdate>());
            return result != null;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return false;
        }
    }
}
