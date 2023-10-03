FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FastDinner.Api/FastDinner.Api.csproj", "FastDinner.Api/"]
COPY ["FastDinner.Application/FastDinner.Application.csproj", "FastDinner.Application/"]
COPY ["FastDinner.Domain/FastDinner.Domain.csproj", "FastDinner.Domain/"]
COPY ["FastDinner.Contracts/FastDinner.Contracts.csproj", "FastDinner.Contracts/"]
COPY ["FastDinner.Infrastructure/FastDinner.Infrastructure.csproj", "FastDinner.Infrastructure/"]
RUN dotnet restore "FastDinner.Api/FastDinner.Api.csproj"
COPY . .
WORKDIR "/src/FastDinner.Api"
RUN dotnet build "FastDinner.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FastDinner.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FastDinner.Api.dll"]