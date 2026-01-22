FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

RUN apt-get update && apt-get install -y openjdk-17-jre
RUN dotnet tool install --global dotnet-sonarscanner
ENV PATH="$PATH:/root/.dotnet/tools"

ARG SONAR_TOKEN
ARG SONAR_PROJECT_KEY="AndrodenBY_Imaginator"
ARG SONAR_ORG="androdenby"

COPY ["Imaginator.csproj", "./"]
RUN dotnet restore "Imaginator.csproj"
COPY . .

RUN dotnet sonarscanner begin \
    /k:"${SONAR_PROJECT_KEY}" \
    /o:"${SONAR_ORG}" \
    /d:sonar.login="${SONAR_TOKEN}" \
    /d:sonar.host.url="https://sonarcloud.io" && \
    dotnet build "Imaginator.csproj" -c $BUILD_CONFIGURATION -o /app/build && \
    dotnet sonarscanner end /d:sonar.login="${SONAR_TOKEN}"

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Imaginator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Imaginator.dll"]
