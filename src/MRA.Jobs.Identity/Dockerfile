FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /src
COPY ["./MRA.Identity.Api/MRA.Identity.Api.csproj", "MRA.Identity.Api/"]
RUN dotnet restore "./MRA.Identity.Api/MRA.Identity.Api.csproj" --verbosity detailed
COPY . .
WORKDIR "./MRA.Identity.Api"
RUN dotnet build "MRA.Identity.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MRA.Identity.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MRA.Identity.Api.dll"]