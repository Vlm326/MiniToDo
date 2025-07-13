FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

COPY *.sln ./
COPY MiniToDo/*.csproj ./MiniToDo/
RUN dotnet restore

COPY. ./
WORKDIR /app/MiniToDo
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 
WORKDIR /app
COPY --from=build /out .
EXPOSE 80
ENTRYPOINT ["dotnet", "MiniToDo.dll"]

