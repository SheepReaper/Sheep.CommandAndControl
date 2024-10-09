FROM mcr.microsoft.com/dotnet/aspnet:9.0@sha256:a54b9e95a8e30028bd0a862dfc1a67ee36e71f7900b25aed37c9119c55682c7f AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0@sha256:ecdbce3ac370d065b4e34bdb367ed8ac10e24d2af630716f9828bf24b360691c AS build
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
