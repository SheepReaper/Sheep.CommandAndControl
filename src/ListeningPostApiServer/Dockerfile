FROM mcr.microsoft.com/dotnet/aspnet:8.0@sha256:b14a41431d252c640aae69b52320e512050fc53bafcb5012b97ccb9ca173c2d5 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:341aa6e8bbaceb1194732328ca27c15bfa9c531687db83a66af3c0c1833f5534 AS build
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
