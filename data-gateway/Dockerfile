#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5019

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["data-gateway/data-gateway.csproj", "data-gateway/"]
RUN dotnet restore "data-gateway/data-gateway.csproj"
COPY . .
WORKDIR "/src/data-gateway"
RUN dotnet build "data-gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "data-gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "data-gateway.dll"]