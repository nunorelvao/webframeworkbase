FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["WebFrameworkBase/WebFrameworkBase.csproj", "WebFrameworkBase/"]
COPY ["FrameworkBaseService/FrameworkBaseService.csproj", "FrameworkBaseService/"]
COPY ["FrameworkBaseRepo/FrameworkBaseRepo.csproj", "FrameworkBaseRepo/"]
COPY ["FrameworkBaseData/FrameworkBaseData.csproj", "FrameworkBaseData/"]
RUN dotnet restore "WebFrameworkBase/WebFrameworkBase.csproj"
COPY . .
WORKDIR "/src/WebFrameworkBase"
RUN dotnet build "WebFrameworkBase.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WebFrameworkBase.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebFrameworkBase.dll"]