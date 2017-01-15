using Dapper;
using Dapper.FastCrud;
using DeerTier.Web.Objects;
using System;
using System.Linq;

namespace DeerTier.Web.Data
{
    public class AccountRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;

        public AccountRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }
        
        public bool ModUser(string username)
        {
            if (!UserExists(username))
            {
                return false;
            }

            using (var conn = _connectionProvider.GetConnection())
            {
                conn.Execute("UPDATE tblUsers SET IsModerator = 1 WHERE Name = @Username", new { Username = username });
            }

            return true;
        }

        public bool DeModUser(string username)
        {
            if (!UserExists(username))
            {
                return false;
            }

            using (var conn = _connectionProvider.GetConnection())
            {
                conn.Execute("UPDATE tblUsers SET IsModerator = 0 WHERE Name = @Username", new { Username = username });
            }

            return true;
        }

        public string[] GetModerators()
        {
            using (var conn = _connectionProvider.GetConnection())
            {
                return conn.Query<string>("SELECT Name FROM tblUsers WHERE IsModerator = 1").ToArray();
            }
        }

        public bool UserExists(string username)
        {
            using (var conn = _connectionProvider.GetConnection())
            {
                return conn.ExecuteScalar<int>("SELECT COUNT(*) FROM tblUsers WHERE Name = @Username", new { Username = username }) > 0;
            }
        }

        public string[] GetUsernames()
        {
            using (var conn = _connectionProvider.GetConnection())
            {
                return conn.Query<string>("SELECT Name FROM tblUsers ORDER BY Name").ToArray();
            }
        }

        public int GetUserId(string username)
        {
            using (var conn = _connectionProvider.GetConnection())
            {
                return conn.ExecuteScalar<int>("SELECT ID FROM tblUsers WHERE Name = @Username", new { Username = username });
            }
        }
        
        public User GetUser(int id)
        {
            using (var conn = _connectionProvider.GetConnection())
            {
                return conn.Get(new User { Id = id });
            }
        }

        public User GetUser(string username)
        {
            using (var conn = _connectionProvider.GetConnection())
            {
                return conn.QueryFirstOrDefault<User>("SELECT * FROM tblUsers WHERE Name = @Username", new { Username = username });
            }
        }

        public void AddUser(User user)
        {
            using (var conn = _connectionProvider.GetConnection())
            {
                conn.Insert(user);
            }
        }
        
        public bool ChangePassword(string username, string newPassword, PasswordType passwordType)
        {
            try
            {
                using (var conn = _connectionProvider.GetConnection())
                {
                    conn.Execute("UPDATE tblUsers SET Password = @Password, PasswordType = @PasswordType WHERE Name = @Username",
                        new { Password = newPassword, PasswordType = passwordType, Username = username });
                }

                return true;
            }
            catch (Exception) { }

            return false;
        }
    }
}
