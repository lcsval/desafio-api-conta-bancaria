# desafio-api-conta-bancaria

# Informações
A api foi construída com .netcore 5 pois a versão 2.2 está descontinuada e não é recomendada seu uso.
Os testes unitários estão na camada de test. Só construí testes para a camada de aplicação, devido ao tempo e também porque é lá que se encontram as regras do negócio.
O banco de dados utilizado foi sql server. Utilizei docker e subi uma instância de um sql server. Seguir o guia abaixo:

* Criar o container docker do sqlserver local:  
  ```
  docker run --name sqlserver -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=123456X@_' -e 'MSSQL_PID=Express' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2017-latest-ubuntu
  ```

* Ver logs do container (verificar se subiu corretamente):
  ```
  docker logs -f sqlserver
  ```

* Criar o DB (executar os passos): 
  ```
  docker exec -it sqlserver "bash"
  ```
  ```
  /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "123456X@_"
  ```
  ```
  CREATE DATABASE desafiodb 
  ```
  ```
  GO
  ```

# Endpoints
1. /api/Bank/CreateAccount (POST) - Informar um nome e o valor inicial -> Cria uma nova conta  
```
{
  "name": "string",
  "balance": 0
}
```

2. /api/Bank/GetAllAccounts (GET) - Nenhum paramêtro precisa ser informado -> Obtém todas as contas existentes

3. /api/Bank/GetAccountById/{id} (GET) - Informar o id da conta -> Obtém a conta informada pelo Id

4. /api/Bank/Extract/{accountId} (GET) - Informar o id da conta -> Obtém um extrato com o saldo atual e todas as movimentações

5. /api/Bank/Deposit (POST) - Informar o id da conta e o valor que será depositado -> Será depositado o valor mediante a taxas na conta informada  
```
{
  "accountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "value": 0
}
```

6. /api/Bank/Withdraw (POST) - Informar o id da conta e o valor que será sacado -> Será sacado o valor mediante a taxas na conta informada  
```
{
  "accountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "value": 0
}
```

7. /api/Bank/Transfer (POST) - Informar o id da conta origem e o id da conta destino, e o valor que será transferido -> Será transferido o valor mediante taxas da conta origem para a conta destino  
```
{
  "originAccountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "destinationAccountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "value": 0
}
```
