using DeerTier.Web.Services;
using DeerTier.Web.Utils;
using log4net;
using System.Web.Mvc;
using System.Web.Security;

namespace DeerTier.Web.Controllers
{
    public class AccountController : BaseController
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(AccountController));

        public AccountController(AccountService accountService, CategoryService categoryService)
            : base(accountService, categoryService) { }

        [HttpGet]
        public ActionResult LogIn()
        {
            if (IsAuthenticated)
            {
                return Redirect("~/");
            }

            return DefaultMessageView();
        }

        [HttpPost]
        public ActionResult LogIn(string username, string password, string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return DefaultMessageView("Please complete all of the fields");
            }

            var user = AccountService.GetUser(username);

            // Ensure user exists
            if (user == null)
            {
                return DefaultMessageView("Username does not exist");
            }

            // Check password
            if (!AccountService.VerifyPassword(user, password))
            {
                return DefaultMessageView("Incorrect password");
            }

            FormsAuthentication.SetAuthCookie(username, true);

            _logger.Debug($"User logged in: [{username}]");

            var redirectUrl = returnUrl ?? "~/";
            return Redirect(redirectUrl);
        }
        
        public ActionResult LogOut(string url)
        {
            FormsAuthentication.SignOut();

            _logger.Debug($"User logged out: [{Username}]");

            return Redirect(url);
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            if (IsAuthenticated)
            {
                return Redirect("~/");
            }

            return DefaultMessageView();
        }

        [HttpPost]
        public ActionResult SignUp(string username, string password, string confirmedPassword)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmedPassword))
            {
                return DefaultMessageView("Please complete all of the fields");
            }

            if (password != confirmedPassword)
            {
                return DefaultMessageView("Passwords do not match");
            }
            
            if (AccountService.UserExists(username))
            {
                return DefaultMessageView("Username already exists");
            }

            AccountService.AddUser(username, password);
            
            FormsAuthentication.SetAuthCookie(username, true);

            _logger.Debug($"User signed up: [{username}]");

            return MessageView("Success", "Registration successful!");
        }

        [HttpGet]
        [Authorize]
        public ActionResult ChangePassword()
        {
            return MessageView();
        }

        [HttpPost]
        [Authorize]
        public ActionResult ChangePassword(string currentPassword, string newPassword, string confirmedNewPassword)
        {
            if (string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmedNewPassword))
            {
                return DefaultMessageView("Please complete all of the fields");
            }
            
            if (newPassword != confirmedNewPassword)
            {
                return DefaultMessageView("New passwords do not match");
            }
            
            if (!AccountService.VerifyPassword(User, currentPassword))
            {
                return DefaultMessageView("Current password is incorrect");
            }

            if (!AccountService.ChangePassword(Username, newPassword))
            {
                return DefaultMessageView("Something went wrong while changing your password. Please try again.");
            }
            
            return MessageView("Success", "Your password has been changed.", "Change Password Success");
        }

        public ActionResult ResetPassword(string username, string adminKey)
        {
            var result = "unauthorized access";
            if (adminKey == ConfigHelper.AdminKey)
            {
                result = AccountService.ResetPassword(username);

                _logger.Debug($"Password has been reset for user: [{username}]");
            }
            return Content(result);
        }
    }
}