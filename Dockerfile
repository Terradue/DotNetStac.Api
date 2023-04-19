# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY src/*.sln .
COPY src/Stac.Api/*.csproj ./Stac.Api/
COPY src/Stac.Api.Clients/*.csproj ./Stac.Api.Clients/
COPY src/Stac.Api.CodeGen/*.csproj ./Stac.Api.CodeGen/
COPY src/Stac.Api.FileSystem/*.csproj ./Stac.Api.FileSystem/
COPY src/Stac.Api.WebApi/*.csproj ./Stac.Api.WebApi/
RUN dotnet restore -r linux-x64 /p:PublishReadyToRun=true ./Stac.Api.FileSystem/Stac.Api.FileSystem.csproj

# copy everything else and build app
COPY src/Stac.Api ./Stac.Api
COPY src/Stac.Api.Clients ./Stac.Api.Clients
COPY src/Stac.Api.CodeGen ./Stac.Api.CodeGen
COPY src/Stac.Api.FileSystem ./Stac.Api.FileSystem
COPY src/Stac.Api.WebApi ./Stac.Api.WebApi
RUN dotnet publish ./Stac.Api.FileSystem/Stac.Api.FileSystem.csproj -f net7.0 -c release -o /app -r linux-x64 --self-contained true

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime-deps:6.0-bullseye-slim-amd64
WORKDIR /app
COPY --from=build /app ./
COPY src/Stac.Api.Tests/Resources/TestCatalogs/CatalogS2L2A /data
ENV STACAPIFS_CatalogRootPath=/data
ENV STACAPIFS_Urls=http://localhost:80
EXPOSE 80
ENTRYPOINT ["./stacapi-fs"]