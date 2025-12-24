# 1. Aşama: Build (İnşaat)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Tüm dosyaları kopyala
COPY . .

# Proje dosyasını bul ve paketleri yükle (Klasör yapısından bağımsız çalışır)
RUN dotnet restore "ArabaKiralama.csproj" || dotnet restore "ArabaKiralama/ArabaKiralama.csproj"

# Yayınla
RUN dotnet publish "ArabaKiralama.csproj" -c Release -o /app/publish || \
    dotnet publish "ArabaKiralama/ArabaKiralama.csproj" -c Release -o /app/publish

# 2. Aşama: Run (Çalıştırma)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Projenin DLL adının doğruluğundan emin ol
ENTRYPOINT ["dotnet", "ArabaKiralama.dll"]