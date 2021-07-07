using System;
using System.Collections.Generic;
using TweetStatsViewer.Interfaces;

namespace TweetStatsViewer.Business
{
    public class ReceivedTweetProcessor : IReceivedTweetProcessor
    {
        private readonly ITweetDataProvider _dataProvider;
        private readonly IEnumerable<ICollectionProcessor> _collectionProcessor;

        public ReceivedTweetProcessor(ITweetDataProvider dataProvider, IEnumerable<ICollectionProcessor> collectionProcessor)
        {
            _dataProvider = dataProvider;
            _collectionProcessor = collectionProcessor;
        }

        public void ProcessTweet(string text, IEnumerable<string> urls, IEnumerable<string> hashtags)
        {
            try
            {
                var timeSpan = DateTime.UtcNow - _dataProvider.InstanceCreatedAtUtc();
                _dataProvider.AddTweet();
                _dataProvider.SetAverageTweetsPerSecond((int)Math.Round(_dataProvider.TotalNumberOfTweets() / timeSpan.TotalSeconds));
                _dataProvider.SetAverageTweetsPerMinute((int)Math.Round(_dataProvider.TotalNumberOfTweets() / timeSpan.TotalMinutes));
                _dataProvider.SetAverageTweetsPerHour((int)Math.Round(_dataProvider.TotalNumberOfTweets() / timeSpan.TotalHours));

                foreach (var processor in _collectionProcessor)
                {
                    processor.ProcessTweet(text, urls, hashtags);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                _dataProvider.AddError(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
#else
                _dataProvider.AddError("Unknown error occurred processing tweet.");
#endif
            }
        }
    }
}
