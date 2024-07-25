# Use the official .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk AS build

WORKDIR /source

# Copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o /app

# Final stage: run the app in a lightweight container
FROM mcr.microsoft.com/dotnet/aspnet
WORKDIR /app
COPY --from=build /app .

# Copy data
COPY data /data

ENV ASPNETCORE_ENVIRONMENT=Development

EXPOSE 8080
ENTRYPOINT ["dotnet", "mvcapi.dll"]
