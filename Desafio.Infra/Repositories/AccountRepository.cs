using Dapper;
using Desafio.Domain.Entities;
using Desafio.Domain.Interfaces.Repositories;
using Desafio.Infra.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Desafio.Infra.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private DbSession _session;

        public AccountRepository(DbSession session)
        {
            _session = session;
        }

        public async Task<Account> Create(Account account)
        {
            var sql = @"
                INSERT INTO accounts
                    (name, balance)
                OUTPUT Inserted.id
                VALUES
                    (@name, @balance)";

            var id = await _session.Connection.ExecuteScalarAsync<Guid>(sql, account, _session.Transaction);
            var result = await _session.Connection.QueryFirstAsync<Account>($"SELECT * FROM accounts WHERE id = '{id}'", null, _session.Transaction);
            return result;
        }

        public async Task<List<Account>> GetAll()
        {
            var sql = $"SELECT * FROM accounts";

            var result = await _session.Connection.QueryAsync<Account>(sql, null, _session.Transaction);

            return result.ToList();
        }

        public async Task<Account> GetById(Guid id)
        {
            var sql = $"SELECT * FROM accounts WHERE id = '{id}'";

            var result = await _session.Connection.QueryAsync<Account>(sql, null, _session.Transaction);

            return result.Any() ? result.FirstOrDefault() : null;
        }

        public async Task<Account> Update(Account account)
        {
            var sql = @"
                UPDATE
                    accounts
                SET
                    name = @name,
                    balance = @balance
                WHERE
                    id = @Id";

            await _session.Connection.ExecuteAsync(sql, account, _session.Transaction);
            var result = await _session.Connection.QueryFirstAsync<Account>($"SELECT * FROM accounts WHERE id = '{account.Id}'", null, _session.Transaction);
            return result;
        }
    }
}
