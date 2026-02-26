FROM mcr.microsoft.com/dotnet/sdk:5.0 AS builder
WORKDIR /usr/src/app

# download deps
COPY hello-world-dotnet.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /usr/src/app

COPY --from=builder /usr/src/app/bin .
ENTRYPOINT ["dotnet", "Release/net5.0/hello-world-dotnet.dll"]
