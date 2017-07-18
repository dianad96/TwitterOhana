using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Tweetinvi.Models;

namespace TwitterOhana.Services
{
    public class CredentialService : ICredentialService
    {
        private IHttpContextAccessor _httpContext;
        private readonly MyConfiguration _options;

        public CredentialService(IHttpContextAccessor httpContext, IOptions<MyConfiguration> optionsAccessor)
        {
            _httpContext = httpContext;
            _options = optionsAccessor.Value;
        }
        public ITwitterCredentials GetUserCredentials()
        {
            var accessToken = _httpContext.HttpContext.User.Claims.ElementAt(0).Value;
            var accessTokenSecret = _httpContext.HttpContext.User.Claims.ElementAt(1).Value;

            return new TwitterCredentials(
                _options.ConsumerKey,
                _options.ConsumerSecret,
                accessToken,
                accessTokenSecret);
        }

        public string getConsumerKey()
        {
            return _options.ConsumerKey;
        }

        public string getConsumerSecret()
        {
            return _options.ConsumerSecret;
        }

        public string getRedirectUrl()
        {
            return _options.RedirectUrl;
        }
    }
}
