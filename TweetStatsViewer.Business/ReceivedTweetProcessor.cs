using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TweetStatsViewer.Interfaces;

namespace TweetStatsViewer.Business
{
    public class ReceivedTweetProcessor : IReceivedTweetProcessor
    {
        private readonly ITweetDataProvider _dataProvider;
        private readonly string _domainRegex = @"https?:\/\/(www\.)?([-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-z]{2,4})\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)";
        private readonly string[] _imageUrls = { "pic.twitter.com", "instagram.com" };

        public ReceivedTweetProcessor(ITweetDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public void ProcessTweet(string text, IEnumerable<string> urls, IEnumerable<string> hashtags)
        {
            var data = _dataProvider.GetData();
            try
            {
                var timeSpan = DateTime.UtcNow - data.InstanceCreatedAtUtc;
                data.TotalNumberOfTweets++;
                data.AverageTweetsPerSecond = (int)Math.Round(data.TotalNumberOfTweets / timeSpan.TotalSeconds);
                data.AverageTweetsPerMinute = (int)Math.Round(data.TotalNumberOfTweets / timeSpan.TotalMinutes);
                data.AverageTweetsPerHour = (int)Math.Round(data.TotalNumberOfTweets / timeSpan.TotalHours);

                if (urls != null)
                {
                    var tweetHasImage = false;
                    foreach (var url in urls)
                    {
                        if (Regex.IsMatch(url, _domainRegex))
                        {
                            var domain = Regex.Match(url, _domainRegex).Groups[2].Value;
                            AddEntry(data.TopDomains, domain);
                        }
                        tweetHasImage = _imageUrls.Any(r => url.Contains(r)) || tweetHasImage;
                    }
                    data.NumberOfTweetsWithUrls++;
                    data.PercentOfTweetsWithUrls = (int)Math.Round(data.NumberOfTweetsWithUrls / (double)data.TotalNumberOfTweets * 100);
                    if (tweetHasImage)
                    {
                        data.NumberOfTweetsWithImageUrls++;
                        data.PercentOfTweetsWithImageUrls = (int)Math.Round(data.NumberOfTweetsWithImageUrls / (double)data.TotalNumberOfTweets * 100);
                    }
                }

                if (hashtags != null)
                {
                    foreach (var hashtag in hashtags)
                    {
                        AddEntry(data.TopHashtags, hashtag);
                    }
                }

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
                            data.NumberOfTweetsWithEmojis++;
                            AddEntry(data.TopEmojis, emoji.Short_name);
                        }
                    }
                    data.PercentOfTweetsWithEmojis = (int)Math.Round(data.NumberOfTweetsWithEmojis / (double)data.TotalNumberOfTweets * 100);
                }
            }
            catch (Exception)
            {
                data.Errors.Add("Unknown error occurred processing tweet.");
            }
        }

        private void AddEntry(IDictionary<string, int> list, string value)
        {
            if (list.ContainsKey(value))
            {
                list[value]++;
            }
            else
            {
                list.Add(value, 1);
            }
        }
    }
}
