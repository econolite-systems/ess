// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using Econolite.Ode.Config.Devices;
using Econolite.Ode.Models.DeviceManager.Db;
using Econolite.Ode.Models.Entities;
using Econolite.Ode.Models.Entities.Types;

namespace Econolite.Ode.Models.Ess.Db;

public class EnvironmentalSensor : Entity
{
    public EnvironmentalSensor()
    {
        Type = new EntityTypeId() {Id = EssTypeId.Id, Name = EssTypeId.Name};
    }

    public string ControllerType { get; set; } = "Ess";
    public Guid DeviceManager { get; set; }
    public Guid Channel { get; set; }
    public CommMode CommMode { get; set; }
    public Controller Controller { get; set; } = new Controller();
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
