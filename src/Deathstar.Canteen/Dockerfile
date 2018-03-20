FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.1.5-aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .
RUN sed -i "s|mongodb://localhost|mongodb://deathstar-database|g" appsettings.json
RUN echo '{"urls":"http://+:80"}' > hosting.json
ENTRYPOINT [ "dotnet", "Deathstar.Canteen.dll" ]
