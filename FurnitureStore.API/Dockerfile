#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy everything
COPY ["FurnitureStore.API/FurnitureStore.API.csproj", "FurnitureStore.API/"]
COPY ["FurnitureStore.Application/FurnitureStore.Application.csproj", "FurnitureStore.Application/"]
COPY ["FurnitureStore.Domain/FurnitureStore.Domain.csproj", "FurnitureStore.Domain/"]
COPY ["FurnitureStore.Contracts/FurnitureStore.Contracts.csproj", "FurnitureStore.Contracts/"]
COPY ["FurnitureStore.Infrastructure/FurnitureStore.Infrastructure.csproj", "FurnitureStore.Infrastructure/"]

# Restore as distinct layers
RUN dotnet restore "FurnitureStore.API/FurnitureStore.API.csproj"

# Copy every else and build
COPY . .
RUN dotnet build "FurnitureStore.API/FurnitureStore.API.csproj" -c Release -o /app/build


# Build and publish a release
RUN dotnet publish "FurnitureStore.API/FurnitureStore.API.csproj" -c Release -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
# Run this to generate it: dotnet dev-certs https -ep cert.pfx -p Test1234!
COPY ["FurnitureStore.API/cert.pfx", "/https/cert.pfx"]
ENTRYPOINT ["dotnet", "FurnitureStore.API.dll"]

