# Dockerfile for deLuca.InventoryManager.Api

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# copy csproj and restore
COPY ["deLuca.InventoryManager.Api/deLuca.InventoryManager.Api.csproj", "deLuca.InventoryManager.Api/"]
RUN dotnet restore "deLuca.InventoryManager.Api/deLuca.InventoryManager.Api.csproj"

# copy everything else and build
COPY . .
WORKDIR "/src/deLuca.InventoryManager.Api"
RUN dotnet publish "deLuca.InventoryManager.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish ./
ENTRYPOINT ["dotnet", "deLuca.InventoryManager.Api.dll"]
