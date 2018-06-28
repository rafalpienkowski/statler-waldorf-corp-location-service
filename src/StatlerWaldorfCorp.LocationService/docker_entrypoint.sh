#!/bin/bash
cd /pipeline/source/app/publish

set -e

until dotnet ef database update; do
>&2 echo "Postgres Server is starting up"
sleep 1
done

>&2 echo "Postgres Server is up - executing command"

dotnet StatlerWaldorfCorp.LocationService.dll