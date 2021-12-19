# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:5.0
COPY . /app
WORKDIR /app
RUN ["dotnet", "restore"]
RUN ["dotnet", "build"]
CMD /bin/bash ./entrypoint.sh
