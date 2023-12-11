FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /App

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src

COPY "Servicios/Catalog/CatalogAPI" "Servicios/Catalog/CatalogAPI"

COPY "Servicios/Services.Common" "Servicios/Services.Common"

RUN dotnet restore "Servicios/Catalog/CatalogAPI/CatalogAPI.sln"

WORKDIR /src/Servicios/Catalog/CatalogAPI
# Build and publish a release
RUN dotnet publish --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /App
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PublicAPI.dll"]