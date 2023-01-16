FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebApp.Api/WebApp.Api.csproj", "WebApp.Api/"]
COPY ["WebApp.Services/WebApp.Services.csproj", "WebApp.Services/"]

RUN dotnet restore "WebApp.Api/WebApp.Api.csproj"
COPY . .
WORKDIR "/src/WebApp.Api"
RUN dotnet build "WebApp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApp.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApp.Api.dll"]