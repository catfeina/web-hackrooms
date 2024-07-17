# Etapa 1: Construcción
FROM mcr.microsoft.com/dotnet/sdk AS build
WORKDIR /src

# Copiar csproj y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar el resto del código y compilar
COPY . ./
RUN dotnet publish -c Release -o /app

# Etapa 2: Ejecución
FROM mcr.microsoft.com/dotnet/aspnet AS runtime
WORKDIR /app
COPY --from=build /app .

ENV ASPNETCORE_ENVIRONMENT=Development
ENV ANGULAR=http://app:4200

# Exponer el puerto de la aplicación
EXPOSE 80

ENTRYPOINT ["dotnet", "minimalapi.dll"]
