# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ProjectNet.sln ./
COPY src ./src
COPY tests ./tests

RUN dotnet restore ProjectNet.sln
RUN dotnet publish src/ProjectNet.Api/ProjectNet.Api.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# Runtime stage (distroless-ish slim image, non-root user).
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

RUN adduser --disabled-password --home /app --gecos "" appuser \
    && chown -R appuser /app
USER appuser

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080 \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 8080
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD wget -qO- http://localhost:8080/swagger/index.html || exit 1

ENTRYPOINT ["dotnet", "ProjectNet.Api.dll"]
