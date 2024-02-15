using Dapper.FastCrud;
using MySqlConnector;
using System.Configuration;
using System.Data;

namespace DeerTier.Web.Data
{
    public class DbConnectionProvider : IDbConnectionProvider
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

        static DbConnectionProvider()
        {
            OrmConfiguration.DefaultDialect = SqlDialect.MySql;
        }

        public IDbConnection GetConnection()
        {
            var connection = new MySqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}