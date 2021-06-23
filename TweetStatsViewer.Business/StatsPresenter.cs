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
            _displayHandler.Clear();
            if (_dataProvider.TotalNumberOfTweets() == 0)
            {
                _displayHandler.WriteLine("Initializing connection to sample stream...");
            }
            else
            {
                _displayHandler.WriteLine($"Total number of tweets: {_dataProvider.TotalNumberOfTweets()}");
                _displayHandler.WriteLine($"Tweets per hour: {_dataProvider.AverageTweetsPerHour()}");
                _displayHandler.WriteLine($"Tweets per minute: {_dataProvider.AverageTweetsPerMinute()}");
                _displayHandler.WriteLine($"Tweets per second: {_dataProvider.AverageTweetsPerSecond()}");
                _displayHandler.WriteLine($"Percent of tweets with emojis: {Math.Round(_dataProvider.PercentOfTweetsWithEmojis() * 100) / 100m}");
                _displayHandler.WriteLine($"Percent of tweets with urls: {Math.Round(_dataProvider.PercentOfTweetsWithUrls() * 100) / 100m}");
                _displayHandler.WriteLine($"Percent of tweets with images: {Math.Round(_dataProvider.PercentOfTweetsWithImages() * 100) / 100m}");
                DisplayList(_dataProvider.GetTopEmojisForDisplay(), "Emojis");
                DisplayList(_dataProvider.GetTopDomainsForDisplay(), "Domains");
                DisplayList(_dataProvider.GetTopHashtagsForDisplay(), "Hashtags");
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
