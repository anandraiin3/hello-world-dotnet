FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /usr/src/app

# Copy project file first for layer-cached restore
# Build context must be the repository root (two levels above this Dockerfile)
COPY src/hello-world-dotnet/hello-world-dotnet.csproj src/hello-world-dotnet/
RUN dotnet restore src/hello-world-dotnet/hello-world-dotnet.csproj

COPY src/hello-world-dotnet/ src/hello-world-dotnet/
RUN dotnet publish src/hello-world-dotnet/hello-world-dotnet.csproj -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /usr/src/app

COPY --from=builder /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "hello-world-dotnet.dll"]
