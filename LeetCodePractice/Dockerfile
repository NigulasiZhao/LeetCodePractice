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
COPY ["LeetCodePractice/LeetCodePractice.csproj", "LeetCodePractice/"]
COPY ["LeetCodePractice.CommonTools/LeetCodePractice.CommonTools.csproj", "LeetCodePractice.CommonTools/"]
COPY ["LeetCodePractice.Model/LeetCodePractice.Model.csproj", "LeetCodePractice.Model/"]
COPY ["LeetCodePractice.IDAL/LeetCodePractice.IDAL.csproj", "LeetCodePractice.IDAL/"]
COPY ["LeetCodePractice.OracleDAL/LeetCodePractice.OracleDAL.csproj", "LeetCodePractice.OracleDAL/"]
COPY ["LeetCodePractice.Database/LeetCodePractice.Database.csproj", "LeetCodePractice.Database/"]
COPY ["LeetCodePractice.SQLServerDAL/LeetCodePractice.SQLServerDAL.csproj", "LeetCodePractice.SQLServerDAL/"]
RUN dotnet restore "LeetCodePractice/LeetCodePractice.csproj"
COPY . .
WORKDIR "/src/LeetCodePractice"
RUN dotnet build "LeetCodePractice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LeetCodePractice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LeetCodePractice.dll"]
