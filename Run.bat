echo Run
start cmd /k dotnet exemplo-dotnet-kafka.dll --urls=http://localhost:5003/
ping 127.0.0.1 -n 30 > nul
start cmd /k dotnet exemplo-dotnet-kafka.dll --urls=http://localhost:5004/
ping 127.0.0.1 -n 30 > nul
start cmd /k dotnet exemplo-dotnet-kafka.dll --urls=http://localhost:5005/
ping 127.0.0.1 -n 30 > nul
start cmd /k dotnet exemplo-dotnet-kafka.dll --urls=http://localhost:5006/