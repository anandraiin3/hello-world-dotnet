FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /usr/src/app

# download deps
COPY hello-world-dotnet.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /usr/src/app

COPY --from=builder /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "hello-world-dotnet.dll"]
