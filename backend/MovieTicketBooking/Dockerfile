# ─── BUILD STAGE ─────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution + project files first for layer-cached restore
COPY *.sln .
COPY MovieBooking.Api/*.csproj MovieBooking.Api/
COPY MovieBooking.Application/*.csproj MovieBooking.Application/
COPY MovieBooking.Domain/*.csproj MovieBooking.Domain/
COPY MovieBooking.Infrastructure/*.csproj MovieBooking.Infrastructure/

RUN dotnet restore

# Copy everything and publish
COPY . .
WORKDIR /src/MovieBooking.Api
RUN dotnet publish -c Release -o /app/publish

# ─── RUNTIME STAGE ───────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

# Create required directories
RUN mkdir -p wwwroot/uploads logs

EXPOSE 8080

ENV ASPNETCORE_HTTP_PORTS=8080

ENTRYPOINT ["dotnet", "MovieBooking.Api.dll"]
