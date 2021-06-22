using Microsoft.Extensions.Configuration;
using TweetStatsViewer.Interfaces;
using TweetStatsViewer.Models;

namespace TweetStatsViewer.Business
{
    public class ConfigurationLoader : IConfigurationLoader
    {
        public TwitterApiCredentials GetTwitterApiCredentials()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var creds = configuration.GetSection("credentials");

            return new TwitterApiCredentials {
                ApiKey = creds["api-key"],
                ApiSecret = creds["api-secret"],
                BearerToken = creds["bearer-token"]
            };
        }
    }
}
