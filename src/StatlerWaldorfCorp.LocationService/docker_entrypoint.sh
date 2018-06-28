#!/bin/bash
cd /pipeline/source/app/publish
dotnet ef database update
dotnet StatlerWaldorfCorp.LocationService.dll