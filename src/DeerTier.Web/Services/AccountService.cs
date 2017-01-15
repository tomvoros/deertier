using DeerTier.Web.Data;
using DeerTier.Web.Objects;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Helpers;

namespace DeerTier.Web.Services
{
    public class AccountService
    {
        private readonly AccountRepository _accountRepository;

        public AccountService(AccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        
        public bool UserExists(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return false;
            }

            return _accountRepository.UserExists(username);
        }
        
        public bool VerifyPassword(User user, string password)
        {
            return VerifyHashedPassword(user.Password, password, user.PasswordType);
        }

        public User GetUser(string username)
        {
            return _accountRepository.GetUser(username);
        }

        public void AddUser(string username, string password)
        {
            var user = new User
            {
                Name = username,
                Password = HashPassword(password, PasswordType.AspNetIdentity),
                PasswordType = PasswordType.AspNetIdentity,
                IsModerator = ModeratorType.NotModerator
            };
            
            _accountRepository.AddUser(user);
        }

        public bool ChangePassword(string username, string newPassword)
        {
            var hashedPassword = HashPassword(newPassword, PasswordType.AspNetIdentity);
            return _accountRepository.ChangePassword(username, hashedPassword, PasswordType.AspNetIdentity);
        }

        public string ResetPassword(string username)
        {
            var newPassword = GenerateRandomPassword();
            ChangePassword(username, newPassword);
            return newPassword;
        }

        public string[] GetUsernames()
        {
            return _accountRepository.GetUsernames();
        }

        public string[] GetModerators()
        {
            return _accountRepository.GetModerators();
        }

        public bool ModUser(string username)
        {
            return _accountRepository.ModUser(username);
        }

        public bool DeModUser(string username)
        {
            return _accountRepository.DeModUser(username);
        }

        /// <summary>
        /// Hash password MD5. This is the insecure, legacy hash algorithm.
        /// </summary>
        /// <param name="password">Password in clear text</param>
        /// <returns>MD5 hash of password</returns>
        private string HashPasswordLegacyMd5(string password)
        {
            var md5 = new MD5CryptoServiceProvider();
            var bytes = Encoding.Default.GetBytes(password);
            return BitConverter.ToString(md5.ComputeHash(bytes));
        }

        private string HashPassword(string password, PasswordType type)
        {
            if (type == PasswordType.LegacyMd5)
            {
                password = HashPasswordLegacyMd5(password);
            }
            
            return Crypto.HashPassword(password);
        }

        private bool VerifyHashedPassword(string hashedPassword, string password, PasswordType type)
        {
            if (type == PasswordType.LegacyMd5)
            {
                password = HashPasswordLegacyMd5(password);
            }

            return Crypto.VerifyHashedPassword(hashedPassword, password);
        }

        private string GenerateRandomPassword()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}