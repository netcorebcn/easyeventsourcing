docker rm -f eventstore-node
docker run -d --name eventstore-node -it -p 2113:2113 -p 1113:1113 eventstore/eventstore
dotnet run -p ./OrderApiSample/OrderApiSample.csproj