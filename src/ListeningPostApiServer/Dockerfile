FROM mcr.microsoft.com/dotnet/aspnet:10.0@sha256:5b201bc57156a9f70d58a86112dc7a8217cf3a885740a8fe914b0782ee09647e AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0@sha256:ed46c853a4f100ba7aab2394e9fb9f9b8989e93346b04632dbce11e5da7623d8 AS build
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
