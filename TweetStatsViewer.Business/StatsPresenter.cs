using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TweetStatsViewer.Interfaces;

namespace TweetStatsViewer.Business
{
    public class StatsPresenter : IStatsPresenter
    {
        private readonly ITweetDataProvider _dataProvider;
        private readonly IDisplayHandler _displayHandler;

        public StatsPresenter(ITweetDataProvider dataProvider, IDisplayHandler displayHandler)
        {
            _dataProvider = dataProvider;
            _displayHandler = displayHandler;
        }

        public void PresentationLoop()
        {
            for (int i = 0; i < 1000000; i++)
            {
                Present();
                Thread.Sleep(3000);
            }
        }

        public void Present()
        {
            var data = _dataProvider.GetData();
            _displayHandler.Clear();
            if (data.TotalNumberOfTweets == 0)
            {
                _displayHandler.WriteLine("Initializing connection to sample stream...");
            }
            else
            {
                _displayHandler.WriteLine($"Total number of tweets: {data.TotalNumberOfTweets}");
                _displayHandler.WriteLine($"Tweets per hour: {data.AverageTweetsPerHour}");
                _displayHandler.WriteLine($"Tweets per minute: {data.AverageTweetsPerMinute}");
                _displayHandler.WriteLine($"Tweets per second: {data.AverageTweetsPerSecond}");
                _displayHandler.WriteLine($"Percent of tweets with emojis: {data.PercentOfTweetsWithEmojis}");
                _displayHandler.WriteLine($"Percent of tweets with urls: {data.PercentOfTweetsWithUrls}");
                _displayHandler.WriteLine($"Percent of tweets with images: {data.PercentOfTweetsWithImageUrls}");
                DisplayList(_dataProvider.GetTopEmojisForDisplay(), "Emojis");
                DisplayList(_dataProvider.GetTopDomainsForDisplay(), "Domains");
                DisplayList(_dataProvider.GetTopHashtagsForDisplay(), "Hashtags");
                _displayHandler.WriteLine("");
                _displayHandler.WriteLine("Press enter to close.");
            }

            foreach (var error in _dataProvider.GetErrorsForDisplay())
            {
                _displayHandler.WriteLine($"Error: {error}");
            }
        }

        private void DisplayList(IDictionary<string, int> list, string name)
        {
            if (list.Any())
            {
                _displayHandler.WriteLine($"*** Top 5 { name } ***");
                foreach (var ec in list.OrderByDescending(r => r.Value).Take(5))
                {
                    _displayHandler.WriteLine($"  {ec.Key} : {ec.Value}");
                }
                _displayHandler.WriteLine("");
            }
        }
    }
}
