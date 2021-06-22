using System;
using System.Linq;
using System.Text.RegularExpressions;
using TweetStatsViewer.Interfaces;

namespace TweetStatsViewer.Business
{
    public class ReceivedTweetProcessor : IReceivedTweetProcessor
    {
        private readonly ITweetDataProvider _dataProvider;

        public ReceivedTweetProcessor(ITweetDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public void ProcessTweet(string text)
        {
            var data = _dataProvider.GetData();
            try
            {
                var timeSpan = DateTime.UtcNow - data.InstanceCreatedAtUtc;
                data.NumberOfTweetsReceived++;
                data.AverageTweetsPerSecond = (int)Math.Round(data.NumberOfTweetsReceived / timeSpan.TotalSeconds);
                data.AverageTweetsPerMinute = (int)Math.Round(data.NumberOfTweetsReceived / timeSpan.TotalMinutes);
                data.AverageTweetsPerHour = (int)Math.Round(data.NumberOfTweetsReceived / timeSpan.TotalHours);

                var emojis = data.EmojiLibrary;

                if (emojis == null)
                {
                    data.Errors.Add("Emoji lookup has not been loaded.");
                }
                else
                {
                    foreach (var emoji in emojis.Where(r => int.TryParse(r.Unified, System.Globalization.NumberStyles.HexNumber, null, out _)))
                    {
                        int value = int.Parse(emoji.Unified, System.Globalization.NumberStyles.HexNumber);
                        string result = char.ConvertFromUtf32(value).ToString();
                        if (Regex.IsMatch(text, result))
                        {
                            data.NumberOfTweetsWithEmojisReceived++;
                            if (data.EmojiCounts.ContainsKey(emoji.Short_name))
                            {
                                data.EmojiCounts[emoji.Short_name]++;
                            }
                            else
                            {
                                data.EmojiCounts.Add(emoji.Short_name, 1);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                data.Errors.Add("Unknown error occurred processing tweet.");
            }
        }
    }
}
