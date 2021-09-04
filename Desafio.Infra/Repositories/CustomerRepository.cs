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
    public class CustomerRepository : ICustomerRepository
    {
        private DbSession _session;

        public CustomerRepository(DbSession session)
        {
            _session = session;
        }

        public async Task<Customer> Create(Customer customer)
        {
            var sql = @"
                INSERT INTO customers
                    (name, lastname, document)
                OUTPUT Inserted.id
                VALUES
                    (@name, @lastname, @document)";

            var id = await _session.Connection.ExecuteScalarAsync<Guid>(sql, customer, _session.Transaction);
            var result = await _session.Connection.QueryFirstAsync<Customer>($"SELECT * FROM customers WHERE id = '{id}'", null, _session.Transaction);
            return result;
        }

        public async Task Delete(Guid id)
        {
            var sql = $"DELETE FROM customers WHERE id = '{id}'";

            await _session.Connection.ExecuteAsync(sql, null, _session.Transaction);
        }

        public async Task<List<Customer>> GetAll()
        {
            var sql = $"SELECT * FROM customers";

            var result = await _session.Connection.QueryAsync<Customer>(sql, null, _session.Transaction);

            return result.ToList();
        }

        public async Task<Customer> GetById(Guid id)
        {
            var sql = $"SELECT * FROM customers WHERE id = '{id}'";

            var result = await _session.Connection.QueryAsync<Customer>(sql, null, _session.Transaction);

            return result.Any() ? result.FirstOrDefault() : null;
        }

        public async Task<Customer> Update(Customer customer)
        {
            var sql = @"
                UPDATE
                    customers
                SET
                    name = @name,
                    lastname = @lastname, 
                    document = @document
                WHERE
                    id = @Id";

            await _session.Connection.ExecuteAsync(sql, customer, _session.Transaction);
            var result = await _session.Connection.QueryFirstAsync<Customer>($"SELECT * FROM customers WHERE id = '{customer.Id}'", null, _session.Transaction);
            return result;
        }
    }
}
