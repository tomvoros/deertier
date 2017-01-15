using System.Web;

namespace DeerTier.Web.Objects
{
    public class UserContext
    {
        private readonly HttpRequestBase _request;

        public UserContext(User user, HttpRequestBase request)
        {
            User = user;
            _request = request;
        }

        public User User { get; private set; }

        public string IpAddress => _request.UserHostAddress;

        public string UserAgent => _request.UserAgent;
    }
}