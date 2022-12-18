# Build solution
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

COPY ["*.sln", "./"]
COPY ["src/Imagination.Infrastructure/*.csproj", "src/Imagination.Infrastructure/"]
COPY ["src/Imagination.Server/*.csproj", "src/Imagination.Server/"]
COPY ["src/Imagination.Tests/*.csproj", "src/Imagination.Tests/"]
COPY ["tools/Imagination/*.csproj", "tools/Imagination/"]
RUN dotnet restore
COPY . .

FROM build AS publish-imagination
WORKDIR "/src/Imagination.Server"
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final-imagination
WORKDIR /app
COPY --from=publish-imagination /app .
ENTRYPOINT ["dotnet", "Imagination.Server.dll"]