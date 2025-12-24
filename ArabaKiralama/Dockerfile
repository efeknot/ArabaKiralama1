# .NET 8 SDK kullanarak build işlemi
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Çalıştırma aşaması
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
# ArabaKiralama.dll kısmının senin proje adınla aynı olduğundan emin ol
ENTRYPOINT ["dotnet", "ArabaKiralama.dll"]