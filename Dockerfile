FROM mcr.microsoft.com/dotnet/sdk:5.0 as build-env
WORKDIR /app

COPY CarsInfo.WebApi/CarsInfo.WebApi.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish CarsInfo.sln -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "CarsInfo.WebApi.dll"]