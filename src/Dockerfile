FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app

# restore
COPY *.sln .
COPY Deathstar.Canteen/*.csproj ./Deathstar.Canteen/
COPY Deathstar.Canteen.Tests/*.csproj ./Deathstar.Canteen.Tests/
RUN dotnet restore
COPY . .

# test
FROM build AS test
WORKDIR /app/Deathstar.Canteen.Tests
RUN dotnet test

# publish
FROM build AS publish
WORKDIR /app/Deathstar.Canteen
RUN dotnet publish -c Release -o out

# run
FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY --from=publish /app/Deathstar.Canteen/out .
ENTRYPOINT [ "dotnet", "Deathstar.Canteen.dll" ]
