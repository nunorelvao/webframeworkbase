FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 15800:80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["FrameworkBase/WebFrameworkBase/WebFrameworkBase.csproj", "FrameworkBase/WebFrameworkBase/"]
COPY ["FrameworkBase/FrameworkBaseService/FrameworkBaseService.csproj", "FrameworkBase/FrameworkBaseService/"]
COPY ["FrameworkBase/FrameworkBaseRepo/FrameworkBaseRepo.csproj", "FrameworkBase/FrameworkBaseRepo/"]
COPY ["FrameworkBase/FrameworkBaseData/FrameworkBaseData.csproj", "FrameworkBase/FrameworkBaseData/"]
RUN dotnet restore "FrameworkBase/WebFrameworkBase/WebFrameworkBase.csproj"
COPY . .
WORKDIR "/src/FrameworkBase/WebFrameworkBase"
RUN dotnet build "WebFrameworkBase.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WebFrameworkBase.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebFrameworkBase.dll"]