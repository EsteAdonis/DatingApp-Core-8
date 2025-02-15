# Stage 1: Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["API.csproj", "src/"]
RUN dotnet restore "src/API.csproj"
COPY . .
WORKDIR /src
RUN dotnet build "API.csproj" -c Development -o /app/build

#Staghe 2: Publish Stage
FROM  build AS publish
WORKDIR /src
RUN dotnet publish "API.csproj" -c Development -o /app/publish

#Stage 3: Run Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
ENV ASPNETCORE_HTTP_PORTS=5000
EXPOSE 5000
WORKDIR /app

# Install SQLite
#RUN apt-get update && apt-get install -y sqlite3 libsqlite3-dev

COPY --from=publish /app/publish .
COPY dating.db ./
ENTRYPOINT [ "dotnet", "API.dll"]

