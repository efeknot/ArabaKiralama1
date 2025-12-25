FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .


RUN dotnet restore "ArabaKiralama.csproj" || dotnet restore "ArabaKiralama/ArabaKiralama.csproj"

# YAYINLAMA:
RUN dotnet publish "ArabaKiralama.csproj" -c Release -o /app/publish || \
    dotnet publish "ArabaKiralama/ArabaKiralama.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ArabaKiralama.dll"]