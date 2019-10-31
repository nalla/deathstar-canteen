FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS restore
WORKDIR /app
COPY *.sln .
COPY Deathstar.Canteen/*.csproj ./Deathstar.Canteen/
COPY Deathstar.Canteen.Tests/*.csproj ./Deathstar.Canteen.Tests/
RUN dotnet restore

FROM restore AS test
COPY . .
WORKDIR /app/Deathstar.Canteen.Tests
RUN dotnet test

FROM test AS publish
WORKDIR /app/Deathstar.Canteen
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
WORKDIR /app
COPY --from=publish /app/Deathstar.Canteen/out .
ENTRYPOINT [ "dotnet", "Deathstar.Canteen.dll" ]
