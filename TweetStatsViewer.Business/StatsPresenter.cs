using System;
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
            if (data.NumberOfTweetsReceived == 0)
            {
                _displayHandler.WriteLine("Initializing connection to sample stream...");
            }
            else
            {
                _displayHandler.WriteLine($"Total number of tweets received: {data.NumberOfTweetsReceived}");
                _displayHandler.WriteLine($"Tweets per hour: {data.AverageTweetsPerHour}");
                _displayHandler.WriteLine($"Tweets per minute: {data.AverageTweetsPerMinute}");
                _displayHandler.WriteLine($"Tweets per second: {data.AverageTweetsPerSecond}");
                _displayHandler.WriteLine($"Number of tweets with emojis received: {data.NumberOfTweetsWithEmojisReceived}");
                if (data.EmojiCounts.Any())
                {
                    _displayHandler.WriteLine($"*** Top 10 Emojis ***");
                    foreach (var ec in data.EmojiCounts.OrderByDescending(r => r.Value).Take(10))
                    {
                        _displayHandler.WriteLine($"  {ec.Key} : {ec.Value}");
                    }
                    _displayHandler.WriteLine("");
                }
                _displayHandler.WriteLine("");
                _displayHandler.WriteLine("Press enter to close.");
            }

            foreach (var error in data.Errors)
            {
                _displayHandler.WriteLine($"Error: {error}");
            }
        }
    }
}
