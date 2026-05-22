FROM mcr.microsoft.com/dotnet/aspnet:8.0@sha256:19be23fe71e885186495d8da1f2d417e553daaa99e16e5148c5ca3ee2f062512 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:fc69dc5e0c9789adaac5c8efce71ead4d016a51318667c4f26ce93574b1b9403 AS build
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
