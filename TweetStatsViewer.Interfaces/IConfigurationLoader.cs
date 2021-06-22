using TweetStatsViewer.Models;

namespace TweetStatsViewer.Interfaces
{
    public interface IConfigurationLoader
    {
        TwitterApiCredentials GetTwitterApiCredentials();
    }
}
