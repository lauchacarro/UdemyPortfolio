#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["UdemyPortfolio.Web/UdemyPortfolio.Web.csproj", "UdemyPortfolio.Web/"]
COPY ["UdemyPortfolio.Services/UdemyPortfolio.Services.csproj", "UdemyPortfolio.Services/"]
COPY ["UdemyPortfolio.Models/UdemyPortfolio.Models.csproj", "UdemyPortfolio.Models/"]
RUN dotnet restore "UdemyPortfolio.Web/UdemyPortfolio.Web.csproj"
COPY . .
WORKDIR "/src/UdemyPortfolio.Web"
RUN dotnet build "UdemyPortfolio.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UdemyPortfolio.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet UdemyPortfolio.Web.dll