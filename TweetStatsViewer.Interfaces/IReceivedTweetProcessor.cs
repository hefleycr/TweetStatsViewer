using System.Collections.Generic;

namespace TweetStatsViewer.Interfaces
{
    public interface IReceivedTweetProcessor
    {
        void ProcessTweet(string text, IEnumerable<string> urls, IEnumerable<string> hashtags);
    }
}
