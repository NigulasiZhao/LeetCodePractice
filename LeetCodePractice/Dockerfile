#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-nanoserver-1903 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_ENVIRONMENT=Release


FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1903 AS build
WORKDIR /src
COPY ["GISWaterSupplyAndSewageServer/GISWaterSupplyAndSewageServer.csproj", "GISWaterSupplyAndSewageServer/"]
COPY ["GISWaterSupplyAndSewageServer.CommonTools/GISWaterSupplyAndSewageServer.CommonTools.csproj", "GISWaterSupplyAndSewageServer.CommonTools/"]
COPY ["GISWaterSupplyAndSewageServer.Model/GISWaterSupplyAndSewageServer.Model.csproj", "GISWaterSupplyAndSewageServer.Model/"]
COPY ["GISWaterSupplyAndSewageServer.IDAL/GISWaterSupplyAndSewageServer.IDAL.csproj", "GISWaterSupplyAndSewageServer.IDAL/"]
COPY ["GISWaterSupplyAndSewageServer.OracleDAL/GISWaterSupplyAndSewageServer.OracleDAL.csproj", "GISWaterSupplyAndSewageServer.OracleDAL/"]
COPY ["GISWaterSupplyAndSewageServer.Database/GISWaterSupplyAndSewageServer.Database.csproj", "GISWaterSupplyAndSewageServer.Database/"]
COPY ["GISWaterSupplyAndSewageServer.SQLServerDAL/GISWaterSupplyAndSewageServer.SQLServerDAL.csproj", "GISWaterSupplyAndSewageServer.SQLServerDAL/"]
RUN dotnet restore "GISWaterSupplyAndSewageServer/GISWaterSupplyAndSewageServer.csproj"
COPY . .
WORKDIR "/src/GISWaterSupplyAndSewageServer"
RUN dotnet build "GISWaterSupplyAndSewageServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GISWaterSupplyAndSewageServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GISWaterSupplyAndSewageServer.dll"]