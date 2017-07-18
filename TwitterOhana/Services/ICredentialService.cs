using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace TwitterOhana.Services
{
    public interface ICredentialService
    {
        ITwitterCredentials GetUserCredentials();
        string getConsumerKey();
        string getConsumerSecret();
        string getRedirectUrl();
    }
}
