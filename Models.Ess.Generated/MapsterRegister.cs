// SPDX-License-Identifier: MIT
// Copyright: 2023 Econolite Systems, Inc.
using System.Reflection;
using Econolite.Ode.Models.Ess.Db;
using Mapster;

namespace Econolite.Ode.Models.Ess;

public class MapsterRegister : ICodeGenerationRegister
{
    public void Register(CodeGenerationConfig config)
    {
        config.AdaptTo("[name]Dto")
            .ApplyDefaultRule();

        config.AdaptFrom("[name]Add", MapType.Map)
            .ApplyDefaultRule()
            .ForType<EnvironmentalSensor>(cfg => { cfg.Ignore(ess => ess.Id); });

        config.AdaptFrom("[name]Update", MapType.MapToTarget)
            .ApplyDefaultRule();

        config.GenerateMapper("[name]Mapper")
            .ForType<EnvironmentalSensor>();
    }
}

internal static class RegisterExtensions
{
    public static AdaptAttributeBuilder ApplyDefaultRule(this AdaptAttributeBuilder builder)
    {
        return builder
            .ForType<EnvironmentalSensor>()
            .ExcludeTypes(type => type.IsEnum)
            .AlterType(type => type.IsEnum || Nullable.GetUnderlyingType(type)?.IsEnum == true, typeof(string))
            .ShallowCopyForSameType(true);
    }
}
