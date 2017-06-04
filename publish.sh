set -e
API_KEY=$1

rm -rf ./src/EasyEventSourcing/nupkgs
dotnet pack ./src/EasyEventSourcing/EasyEventSourcing.csproj --output nupkgs
dotnet nuget push ./src/EasyEventSourcing/nupkgs/*.nupkg  \
-s https://www.nuget.org/api/v2/package \
-k $API_KEY