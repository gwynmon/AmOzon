FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["AmOzon.API/AmOzon.API.csproj", "AmOzon.API/"]
COPY ["AmOzon.Application/AmOzon.Application.csproj", "AmOzon.Application/"]
COPY ["AmOzon.Domain/AmOzon.Domain.csproj", "AmOzon.Domain/"]
COPY ["AmOzon.Persistence/AmOzon.Persistence.csproj", "AmOzon.Persistence/"]
COPY ["AmOzon.Contracts/AmOzon.Contracts.csproj", "AmOzon.Contracts/"]

RUN dotnet restore "AmOzon.API/AmOzon.API.csproj"

COPY . .

WORKDIR "/src/AmOzon.API"
RUN dotnet build "AmOzon.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AmOzon.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "AmOzon.API.dll"]