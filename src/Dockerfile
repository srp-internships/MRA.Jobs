#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 9595

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MRA.Jobs.Api/WebUI/MRA.Jobs.Web.csproj", "MRA.Jobs.Api/WebUI/"]
COPY ["MRA.Jobs.Api/Application/MRA.Jobs.Application.csproj", "MRA.Jobs.Api/Application/"]
COPY ["MRA.Jobs.Identity/MRA.Identity.Application.Contract/MRA.Identity.Application.Contract.csproj", "MRA.Jobs.Identity/MRA.Identity.Application.Contract/"]
COPY ["MRA.Jobs.Api/Application.Contracts/MRA.Jobs.Application.Contracts.csproj", "MRA.Jobs.Api/Application.Contracts/"]
COPY ["MRA.Jobs.Api/Domain/MRA.Jobs.Domain.csproj", "MRA.Jobs.Api/Domain/"]
COPY ["MRA.Jobs.Api/Infrastructure/MRA.Jobs.Infrastructure.csproj", "MRA.Jobs.Api/Infrastructure/"]
RUN dotnet restore "./MRA.Jobs.Api/WebUI/./MRA.Jobs.Web.csproj"
COPY . .
WORKDIR "/src/MRA.Jobs.Api/WebUI"
RUN dotnet build "./MRA.Jobs.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MRA.Jobs.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MRA.Jobs.Web.dll"]