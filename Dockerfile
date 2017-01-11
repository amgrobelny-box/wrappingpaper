#docker build . -t wrappingpaper
#docker images
#docker run -it -d -p 80:5000 wrappingpaper
#docker ps

FROM microsoft/dotnet:latest

COPY . /app

WORKDIR /app

RUN ["dotnet", "restore"]

RUN ["dotnet", "build"]

EXPOSE 5000/tcp

CMD ["dotnet", "run", "--server.urls", "http://*:5000"]
