using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DeerTier.Web.Data
{
    public class DbConnectionProvider : IDbConnectionProvider
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

        public IDbConnection GetConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}