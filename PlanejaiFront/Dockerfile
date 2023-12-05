FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

WORKDIR /app

EXPOSE 5293

RUN apt-get update \
 && apt-get install -y --allow-unauthenticated \
 libc6-dev \
 libgdiplus \
 libx11-dev \
 && rm -rf /var/lib/apt/lists/*

ENV DISPLAY :99
ENV ASPNETCORE_URLS=http://+:5293
ENV DisableHttpsRedirection=true
ENV POSTGRES_PASSWORD: postgres

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./PlanejaiFront.csproj", "./"]

RUN dotnet restore "./PlanejaiFront.csproj"
COPY . .

WORKDIR "/src/."

RUN dotnet build "PlanejaiFront.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PlanejaiFront.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PlanejaiFront.dll"]

ENV TZ=America/Sao_Paulo
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone