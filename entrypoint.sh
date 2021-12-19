#!/bin/bash

set -e

export PATH="$PATH:/root/.dotnet/tools"
dotnet tool install --global dotnet-ef

until dotnet ef database update --project PakaUsers.Model; do
>&2 echo "Postgresql is starting up"
sleep 1
done

>&2 echo "Postgresql - executing command"

dotnet run --project PakaUsers