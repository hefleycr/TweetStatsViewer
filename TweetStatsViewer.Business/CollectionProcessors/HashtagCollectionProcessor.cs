using System.Collections.Generic;
using TweetStatsViewer.Interfaces;

namespace TweetStatsViewer.Business.CollectionProcessors
{
    public class HashtagCollectionProcessor : ICollectionProcessor
    {
        private readonly ITweetDataProvider _dataProvider;

        public HashtagCollectionProcessor(ITweetDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public void ProcessTweet(string text, IEnumerable<string> urls, IEnumerable<string> hashtags)
        {
            if (hashtags != null)
            {
                foreach (var hashtag in hashtags)
                {
                    _dataProvider.AddHashtag(hashtag);
                }
            }
        }
    }
}
