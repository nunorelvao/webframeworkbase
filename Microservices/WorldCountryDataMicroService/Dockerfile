FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Microservices/WorldCountryDataMicroService/WorldCountryDataMicroService.csproj", "Microservices/WorldCountryDataMicroService/"]
RUN dotnet restore "Microservices/WorldCountryDataMicroService/WorldCountryDataMicroService.csproj"
COPY . .
WORKDIR "/src/Microservices/WorldCountryDataMicroService"
RUN dotnet build "WorldCountryDataMicroService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WorldCountryDataMicroService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WorldCountryDataMicroService.dll"]