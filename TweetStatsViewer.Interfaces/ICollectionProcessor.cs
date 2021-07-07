using System.Collections.Generic;

namespace TweetStatsViewer.Interfaces
{
    public interface ICollectionProcessor
    {
        void ProcessTweet(string text, IEnumerable<string> urls, IEnumerable<string> hashtags);
    }
}
