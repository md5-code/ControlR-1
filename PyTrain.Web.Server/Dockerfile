# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
RUN apt update
RUN apt -y install curl
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 9222

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . .
RUN dotnet restore "./PyTrain.Web.Server/PyTrain.Web.Server.csproj"
WORKDIR "/src/PyTrain.Web.Server"
RUN dotnet build "./PyTrain.Web.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
ARG CURRENT_VERSION=1.0.2
RUN dotnet publish "./PyTrain.Web.Server.csproj" -c $BUILD_CONFIGURATION -p:Version=$CURRENT_VERSION -p:FileVersion=$CURRENT_VERSION -o /app/publish /p:UseAppHost=false

FROM build AS pack-installers
ARG CURRENT_VERSION=1.0.2
WORKDIR /src
RUN apt-get update && apt-get install -y --no-install-recommends zip && rm -rf /var/lib/apt/lists/*
RUN dotnet publish ./PyTrain.Agent.Installer/ -c Release -r win-x64 --self-contained \
      -o /artifacts/downloads/win-x64/ \
      -p:PublishSingleFile=true -p:UseAppHost=true -p:EnableCompressionInSingleFile=true \
      -p:Version=$CURRENT_VERSION -p:FileVersion=$CURRENT_VERSION -p:DebugType=none
RUN dotnet publish ./PyTrain.Agent/ -c Release -r win-x64 \
      -o /artifacts/agent/win-x64/ \
      -p:PublishSingleFile=true -p:UseAppHost=true \
      -p:IncludeAllContentForSelfExtract=true -p:EnableCompressionInSingleFile=true \
      -p:IncludeAppSettingsInSingleFile=true \
      -p:Version=$CURRENT_VERSION -p:FileVersion=$CURRENT_VERSION -p:DebugType=none
RUN dotnet publish ./PyTrain.DesktopClient/ -c Release -f net10.0-windows8.0 -r win-x64 --self-contained \
      -o /artifacts/desktop/win-x64/ \
      -p:Version=$CURRENT_VERSION -p:FileVersion=$CURRENT_VERSION -p:DebugType=none
RUN mkdir -p /tmp/bundle/win-x64/DesktopClient && \
    cp /artifacts/agent/win-x64/*.exe /tmp/bundle/win-x64/ && \
    cp -r /artifacts/desktop/win-x64/. /tmp/bundle/win-x64/DesktopClient/ && \
    (cd /tmp/bundle/win-x64 && zip -r /artifacts/downloads/win-x64/PyTrain.Agent.bundle.zip .)
RUN echo "$CURRENT_VERSION" > /artifacts/downloads/Version.txt

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=pack-installers --chown=$APP_UID /artifacts/downloads/ /app/wwwroot/downloads/
RUN mkdir -p /app/novnc
ENTRYPOINT ["dotnet", "PyTrain.Web.Server.dll"]

HEALTHCHECK \
  CMD curl -f http://localhost:8080/health || exit 1