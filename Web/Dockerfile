# Build from the repository root: docker build -f Web/Dockerfile .

# Build stage: restore first, so the packages layer is cached until a .csproj changes
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY Domain/Domain.csproj Domain/
COPY Application/Application.csproj Application/
COPY Infrastructure/Infrastructure.csproj Infrastructure/
COPY Web/Web.csproj Web/
RUN dotnet restore Web/Web.csproj
COPY Domain/ Domain/
COPY Application/ Application/
COPY Infrastructure/ Infrastructure/
COPY Web/ Web/
RUN dotnet publish Web/Web.csproj --configuration Release --no-restore --output /app

# Runtime stage: only the ASP.NET Core runtime, running as the image's non-root "app" user
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
# Serilog's file sink writes to /app/logs, so that folder must belong to the non-root user
RUN mkdir logs && chown "$APP_UID" logs
USER $APP_UID
EXPOSE 8080
ENTRYPOINT ["dotnet", "Web.dll"]
