#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SF.IP.InsurancePolicy.DemoAPI/SF.IY.InsurancePolicy.DemoAPI.csproj", "SF.IP.InsurancePolicy.DemoAPI/"]
COPY ["SF.IP.Infrastructure/SF.IP.Infrastructure.csproj", "SF.IP.Infrastructure/"]
COPY ["SF.IP.Application/SF.IP.Application.csproj", "SF.IP.Application/"]
COPY ["SF.IP.Domain/SF.IP.Domain.csproj", "SF.IP.Domain/"]
RUN dotnet restore "SF.IP.InsurancePolicy.DemoAPI/SF.IY.InsurancePolicy.DemoAPI.csproj"
COPY . .
WORKDIR "/src/SF.IP.InsurancePolicy.DemoAPI"
RUN dotnet build "SF.IY.InsurancePolicy.DemoAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SF.IY.InsurancePolicy.DemoAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SF.IY.InsurancePolicy.DemoAPI.dll"]
