FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY SubscriptionManager.slnx ./
COPY src/SubscriptionManager.Api/SubscriptionManager.Api.csproj src/SubscriptionManager.Api/
COPY src/SubscriptionManager.Application/SubscriptionManager.Application.csproj src/SubscriptionManager.Application/
COPY src/SubscriptionManager.Domain/SubscriptionManager.Domain.csproj src/SubscriptionManager.Domain/
COPY src/SubscriptionManager.Infrastructure/SubscriptionManager.Infrastructure.csproj src/SubscriptionManager.Infrastructure/
COPY src/SubscriptionManager.Migrations.Sqlite/SubscriptionManager.Migrations.Sqlite.csproj src/SubscriptionManager.Migrations.Sqlite/
COPY src/SubscriptionManager.Migrations.SqlServer/SubscriptionManager.Migrations.SqlServer.csproj src/SubscriptionManager.Migrations.SqlServer/

RUN dotnet restore src/SubscriptionManager.Api/SubscriptionManager.Api.csproj

COPY . .
RUN dotnet publish src/SubscriptionManager.Api/SubscriptionManager.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "SubscriptionManager.Api.dll"]