using System.Data;

namespace DeerTier.Web.Data
{
    public interface IDbConnectionProvider
    {
        IDbConnection GetConnection();
    }
}