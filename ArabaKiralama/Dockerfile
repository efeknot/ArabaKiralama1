FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Önce proje dosyasını kopyala ve paketleri çek
COPY *.csproj .
RUN dotnet restore

# Kalan her şeyi kopyala ve yayınla
COPY . .
RUN dotnet publish -c Release -o /app

# Çalıştırma aşaması
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "ArabaKiralama.dll"]