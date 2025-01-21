FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

ARG MICROSERVICE_NAME

WORKDIR /src
COPY ["Services/${MICROSERVICE_NAME}", "Services/${MICROSERVICE_NAME}/"]
COPY ["Services/Shared", "Services/Shared/"]
RUN dotnet restore "Services/${MICROSERVICE_NAME}/${MICROSERVICE_NAME}.csproj"

WORKDIR "/src/Services/${MICROSERVICE_NAME}"
RUN dotnet build "${MICROSERVICE_NAME}.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "${MICROSERVICE_NAME}.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final

ARG MICROSERVICE_NAME 

WORKDIR /app
COPY --from=publish /app/publish .

# Create an environment variable to hold the executable name
ENV APP_EXE="${MICROSERVICE_NAME}.dll" 

# Build a shell script to launch the application
RUN echo "#!/bin/bash \n dotnet \${APP_EXE}" > ./entrypoint.sh
RUN chmod +x ./entrypoint.sh

# Run the generated shell script.
ENTRYPOINT ["./entrypoint.sh"]

RUN apt-get update 
RUN apt-get --yes install curl
HEALTHCHECK --interval=5s --timeout=10s --retries=3 CMD curl --silent --fail http://localhost:8080/healthz/live || exit 1