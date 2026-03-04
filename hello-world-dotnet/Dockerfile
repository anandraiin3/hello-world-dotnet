FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /usr/src/app

# download deps
COPY hello-world-dotnet.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /usr/src/app

COPY --from=builder /usr/src/app/bin .
EXPOSE 80
ENTRYPOINT ["dotnet", "Release/net5.0/hello-world-dotnet.dll"]
