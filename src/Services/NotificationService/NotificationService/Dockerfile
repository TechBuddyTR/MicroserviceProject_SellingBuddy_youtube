#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/NotificationService/NotificationService/NotificationService.csproj", "src/Services/NotificationService/NotificationService/"]
COPY ["src/BuildingBlocks/EventBus/EventBus.Factory/EventBus.Factory.csproj", "src/BuildingBlocks/EventBus/EventBus.Factory/"]
COPY ["src/BuildingBlocks/EventBus/EventBus.AzureServiceBus/EventBus.AzureServiceBus.csproj", "src/BuildingBlocks/EventBus/EventBus.AzureServiceBus/"]
COPY ["src/BuildingBlocks/EventBus/EventBus.Base/EventBus.Base.csproj", "src/BuildingBlocks/EventBus/EventBus.Base/"]
COPY ["src/BuildingBlocks/EventBus/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj", "src/BuildingBlocks/EventBus/EventBus.RabbitMQ/"]
RUN dotnet restore "src/Services/NotificationService/NotificationService/NotificationService.csproj"
COPY . .
WORKDIR "/src/src/Services/NotificationService/NotificationService"
RUN dotnet build "NotificationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NotificationService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=publish /app/publish .
#COPY bin/Release/net5.0/publish/ App/
ENTRYPOINT ["dotnet", "NotificationService.dll"]