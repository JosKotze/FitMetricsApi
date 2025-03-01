# Base image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the entire application
COPY . ./
RUN dotnet publish -c Release -o out

# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 5000
ENTRYPOINT ["dotnet", "Strava2ExcelWebApiBackend.dll", "--urls", "http://0.0.0.0:5000"]
