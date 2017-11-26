using DeerTier.Web.Models;
using DeerTier.Web.Objects;
using DeerTier.Web.Services;
using System;
using System.Web.Mvc;

namespace DeerTier.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly Lazy<User> _user;
        private readonly Lazy<UserContext> _userContext;

        public BaseController(AccountService accountService, CategoryService categoryService)
        {
            AccountService = accountService;
            CategoryService = categoryService;

            _user = new Lazy<User>(GetUser);
            _userContext = new Lazy<UserContext>(GetUserContext);
        }

        protected AccountService AccountService { get; private set; }
        protected CategoryService CategoryService { get; private set; }

        protected TViewModel CreateViewModel<TViewModel>()
            where TViewModel : PageViewModel, new()
        {
            var viewModel = new TViewModel
            {
                IsAuthenticated = IsAuthenticated,
                IsModerator = IsModerator,
                Username = Username,
                MainCategories = new MainCategoriesModel
                {
                    Sections = CategoryService.SectionModels
                }
            };

            return viewModel;
        }
        
        protected ActionResult MessageView(string viewName = null, string message = null, string title = null)
        {
            var viewModel = CreateViewModel<MessagePageViewModel>();
            viewModel.Message = message;
            viewModel.Title = title;

            if (viewName != null)
            {
                return View(viewName, viewModel);
            }
            else
            {
                return View(viewModel);
            }
        }

        protected ActionResult DefaultMessageView(string message = null)
        {
            return MessageView(message: message);
        }

        public new User User => _user.Value;

        public bool IsAuthenticated => User != null;

        public bool IsModerator => User != null && User.IsModerator != ModeratorType.NotModerator;

        public string Username => User?.Name;

        public int UserId => User != null ? User.Id : 0;

        public UserContext UserContext => _userContext.Value;

        protected virtual User GetUser()
        {
            var baseUser = base.User;
            if (baseUser != null && baseUser.Identity != null && baseUser.Identity.IsAuthenticated && !string.IsNullOrWhiteSpace(baseUser.Identity.Name))
            {
                return AccountService.GetUser(baseUser.Identity.Name);
            }

            return null;
        }

        private UserContext GetUserContext()
        {
            return new UserContext(User, Request);
        }
    }
}