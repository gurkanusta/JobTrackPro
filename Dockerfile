FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["JobTrackPro.Api/JobTrackPro.Api.csproj", "JobTrackPro.Api/"]
COPY ["JobTrackPro.Application/JobTrackPro.Application.csproj", "JobTrackPro.Application/"]
COPY ["JobTrackPro.Domain/JobTrackPro.Domain.csproj", "JobTrackPro.Domain/"]
COPY ["JobTrackPro.Infrastructure/JobTrackPro.Infrastructure.csproj", "JobTrackPro.Infrastructure/"]
RUN dotnet restore "JobTrackPro.Api/JobTrackPro.Api.csproj"
COPY . .
WORKDIR "/src/JobTrackPro.Api"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JobTrackPro.Api.dll"]