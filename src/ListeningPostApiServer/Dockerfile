FROM mcr.microsoft.com/dotnet/aspnet:8.0@sha256:7882d3eee2527584cc9db7b2f26540c27dba4462ec565b8cb160a44874a65b97 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:02fdc848bbda5d57d9211a72c99bd665b421206002d66b8bc2cc0b2297c227fa AS build
WORKDIR /src
COPY ["ListeningPostApiServer/ListeningPostApiServer.csproj" "ListeningPostApiServer/"]
RUN dotnet restore "ListeningPostApiServer/ListeningPostApiServer.csproj"
COPY . .
WORKDIR "/src/ListeningPostApiServer"
RUN dotnet build "ListeningPostApiServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ListeningPostApiServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ListeningPostApiServer.dll"]
