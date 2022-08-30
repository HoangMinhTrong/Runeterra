FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ApiGateways/Ocelot.ApiGw/Ocelot.ApiGw.csproj", "Ocelot.ApiGw/"]
RUN dotnet restore "ApiGateways/Ocelot.ApiGw/Ocelot.ApiGw.csproj"
COPY . .
WORKDIR "/src/Ocelot.ApiGw"
RUN dotnet build "Ocelot.ApiGw.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ocelot.ApiGw.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ocelot.ApiGw.dll"]
