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
    public class AccountRecordRepository : IAccountRecordRepository
    {
        private DbSession _session;

        public AccountRecordRepository(DbSession session)
        {
            _session = session;
        }

        public async Task<AccountRecord> Create(AccountRecord accRecord)
        {
            var sql = @"
                INSERT INTO accountRecords
                    (accountid, value, type, tax, totalvalue)
                OUTPUT Inserted.id
                VALUES
                    (@accountid, @value, @type, @tax, @totalvalue)";

            var id = await _session.Connection.ExecuteScalarAsync<Guid>(sql, accRecord, _session.Transaction);
            var result = await _session.Connection.QueryFirstAsync<AccountRecord>($"SELECT * FROM accountrecords WHERE id = '{id}'", null, _session.Transaction);
            return result;
        }

        public async Task<List<AccountRecord>> GetAll(Guid accountId)
        {
            var sql = $"SELECT * FROM accountrecords where accountId = '{accountId}'";

            var result = await _session.Connection.QueryAsync<AccountRecord>(sql, null, _session.Transaction);

            return result.ToList();
        }

        public async Task<AccountRecord> GetById(Guid id)
        {
            var sql = $"SELECT * FROM customers WHERE id = '{id}'";

            var result = await _session.Connection.QueryAsync<AccountRecord>(sql, null, _session.Transaction);

            return result.Any() ? result.FirstOrDefault() : null;
        }
    }
}
