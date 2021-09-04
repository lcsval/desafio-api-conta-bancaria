# desafio-api-conta-bancaria




Criar o container docker do mssql local: docker run --name sqlserver -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=123456X@_' -e 'MSSQL_PID=Express' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2017-latest-ubuntu

Ver logs do container docker logs -f sqlserver

Criar DB (executar os passos) 1 - docker exec -it sqlserver "bash" 2 - /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "123456X@_" 3 - CREATE DATABASE desafiodb 4 - escrever: GO
