FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["AmOzon.Web/AmOzon.Web.csproj", "AmOzon.Web/"]
COPY ["AmOzon.Domain/AmOzon.Domain.csproj", "AmOzon.Domain/"]
COPY ["AmOzon.Contracts/AmOzon.Contracts.csproj", "AmOzon.Contracts/"]

RUN dotnet restore "AmOzon.Web/AmOzon.Web.csproj"

COPY . .

WORKDIR "/src/AmOzon.Web"
RUN dotnet publish "AmOzon.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "AmOzon.Web.dll"]
