FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /App

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src

COPY "Servicios/Identity/IdentityAPI" "Servicios/Identity/IdentityAPI"

COPY "Servicios/Services.Common" "Servicios/Services.Common"

RUN dotnet restore "Servicios/Identity/IdentityAPI/IdentityAPI.sln"

WORKDIR /src/Servicios/Identity/IdentityAPI
# Build and publish a release
RUN dotnet publish --no-restore -c Release -o /App

FROM build AS publish

FROM base AS final
WORKDIR /App
COPY --from=publish /App .
ENTRYPOINT ["dotnet", "PublicAPI.dll"]