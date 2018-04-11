FROM microsoft/dotnet:latest
WORKDIR /app
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "MongoReadyRoll.dll"]
