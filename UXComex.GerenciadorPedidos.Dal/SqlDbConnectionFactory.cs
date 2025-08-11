using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using UXComex.Domain.Interfaces;

namespace UXComex.Dal
{
    /// <summary>
    /// A concrete implementation of IDbConnectionFactory for SQL Server.
    /// </summary>
    public class SqlDbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public SqlDbConnectionFactory(IConfiguration configuration)
        {
            // Load the connection string from appsettings.json.
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}