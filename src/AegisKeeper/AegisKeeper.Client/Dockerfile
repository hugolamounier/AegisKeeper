FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/AegisKeeper/AegisKeeper.Client/AegisKeeper.Client.csproj", "src/AegisKeeper/AegisKeeper.Client/"]
RUN dotnet restore "src/AegisKeeper/AegisKeeper.Client/AegisKeeper.Client.csproj"
COPY . .
WORKDIR "/src/src/AegisKeeper/AegisKeeper.Client"
RUN dotnet build "AegisKeeper.Client.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AegisKeeper.Client.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AegisKeeper.Client.dll"]
