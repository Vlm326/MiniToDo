# Базовый образ для запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .

# Установим EF Tools (если ты хочешь создавать миграции внутри build-этапа)
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Финальный образ
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS final
WORKDIR /app

# Установка EF Tools
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Копируем собранное приложение
COPY --from=build /app/publish .

# Копируем исходный код для миграций
COPY --from=build /src ./src

# Перейдём в папку, где лежит .csproj
