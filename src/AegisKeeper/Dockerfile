FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

RUN apt-get update && apt-get install -y \
    libodbc1 \
    unixodbc \
    && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/AegisKeeper/AegisKeeper.csproj", "src/AegisKeeper/"]
RUN dotnet restore "src/AegisKeeper/AegisKeeper.csproj"
COPY . .
WORKDIR "/src/src/AegisKeeper"
RUN dotnet build "AegisKeeper.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AegisKeeper.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
RUN apk add --no-cache tzdata
RUN apk add --no-cache icu-libs

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AegisKeeper.dll"]
