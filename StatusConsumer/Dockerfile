# SPDX-License-Identifier: MIT
# Copyright: 2023 Econolite Systems, Inc.

#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ENV SolutionDir /src
WORKDIR /src
COPY ["StatusConsumer/StatusConsumer.csproj", "StatusConsumer/"]
COPY ["common/Extensions.AspNet/Extensions.AspNet.csproj", "common/Extensions.AspNet/"]
COPY ["common/Authorization/Authorization.csproj", "common/Authorization/"]
COPY ["common/Extensions/Extensions.csproj", "common/Extensions/"]
COPY ["common/Monitoring.HealthChecks.Kafka/Monitoring.HealthChecks.Kafka.csproj", "common/Monitoring.HealthChecks.Kafka/"]
COPY ["common/Messaging/Messaging.csproj", "common/Messaging/"]
COPY ["common/Monitoring/Monitoring.csproj", "common/Monitoring/"]
COPY ["common/Monitoring.HealthChecks.Redis/Monitoring.HealthChecks.Redis.csproj", "common/Monitoring.HealthChecks.Redis/"]
COPY ["status/Status.Ess.Cache/Status.Ess.Cache.csproj", "status/Status.Ess.Cache/"]
COPY ["status/Status.Ess/Status.Ess.csproj", "status/Status.Ess/"]
COPY ["status/Status.Common/Status.Common.csproj", "status/Status.Common/"]
COPY ["status/Status.Ess.Messaging/Status.Ess.Messaging.csproj", "status/Status.Ess.Messaging/"]
COPY ["status/Status.Common.Messaging/Status.Common.Messaging.csproj", "status/Status.Common.Messaging/"]

# Generate model files
RUN dotnet new tool-manifest
RUN dotnet tool install Mapster.Tool
COPY ["device-manager/Models.DeviceManager.Generated/Models.DeviceManager.Generated.csproj", "device-manager/Models.DeviceManager.Generated/"]
COPY ["device-manager/Models.DeviceManager/Models.DeviceManager.csproj", "device-manager/Models.DeviceManager/"]
COPY ["Models.Ess/Models.Ess.csproj", "Models.Ess/"]
RUN dotnet build ./device-manager/Models.DeviceManager/ -c Release -o /app/build
RUN dotnet build ./Models.Ess/ -c Release -o /app/build
# End generate

RUN dotnet restore "StatusConsumer/StatusConsumer.csproj"
COPY . .
WORKDIR "/src/StatusConsumer"
RUN dotnet build "StatusConsumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StatusConsumer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StatusConsumer.dll"]