#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["VehicleTracking.IdentityServer.Web/VehicleTracking.IdentityServer.Web.csproj", "VehicleTracking.IdentityServer.Web/"]
RUN dotnet restore "VehicleTracking.IdentityServer.Web/VehicleTracking.IdentityServer.Web.csproj"
COPY . .
WORKDIR "/src/VehicleTracking.IdentityServer.Web"
RUN dotnet build "VehicleTracking.IdentityServer.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VehicleTracking.IdentityServer.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VehicleTracking.IdentityServer.Web.dll"]