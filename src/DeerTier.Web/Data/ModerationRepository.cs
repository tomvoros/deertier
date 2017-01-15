using Dapper.FastCrud;
using DeerTier.Web.Objects;
using System.Linq;

namespace DeerTier.Web.Data
{
    public class ModerationRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;

        public ModerationRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public void LogModerationAction(ModerationAction action)
        {
            using (var conn = _connectionProvider.GetConnection())
            {
                conn.Insert(action);
            }
        }

        public ModerationAction[] GetModerationLog()
        {
            using (var conn = _connectionProvider.GetConnection())
            {
                return conn.Find<ModerationAction>().ToArray();
            }
        }
    }
}
