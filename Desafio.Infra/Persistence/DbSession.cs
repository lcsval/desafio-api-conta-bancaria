using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Desafio.Infra.Persistence
{
    public sealed class DbSession : IDisposable
    {
        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; set; }
        private IConfiguration _config { get; }

        public DbSession(IConfiguration config)
        {
            _config = config;

            var conn = _config["DB:ConnectionString"];
            if (conn != null && conn != string.Empty)
            {
                Connection = new SqlConnection(conn);
                Connection.Open();
            }
        }

        public void Dispose() => Connection?.Dispose();
    }
}
