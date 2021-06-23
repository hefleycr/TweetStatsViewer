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
            try
            {
                var timeSpan = DateTime.UtcNow - _dataProvider.InstanceCreatedAtUtc();
                _dataProvider.AddTweet();
                _dataProvider.SetAverageTweetsPerSecond((int)Math.Round(_dataProvider.TotalNumberOfTweets() / timeSpan.TotalSeconds));
                _dataProvider.SetAverageTweetsPerMinute((int)Math.Round(_dataProvider.TotalNumberOfTweets() / timeSpan.TotalMinutes));
                _dataProvider.SetAverageTweetsPerHour((int)Math.Round(_dataProvider.TotalNumberOfTweets() / timeSpan.TotalHours));

                if (urls != null)
                {
                    var tweetHasImage = false;
                    foreach (var url in urls)
                    {
                        if (Regex.IsMatch(url, _domainRegex))
                        {
                            var domain = Regex.Match(url, _domainRegex).Groups[2].Value;
                            _dataProvider.AddDomain(domain);
                        }
                        tweetHasImage = _imageUrls.Any(r => url.Contains(r)) || tweetHasImage;
                    }
                    _dataProvider.AddTweetWithUrl();
                    _dataProvider.SetPercentOfTweetsWithUrls(_dataProvider.NumberOfTweetsWithUrls() / (decimal)_dataProvider.TotalNumberOfTweets() * 100);
                    if (tweetHasImage)
                    {
                        _dataProvider.AddTweetWithImage();
                        _dataProvider.SetPercentOfTweetsWithImages(_dataProvider.NumberOfTweetsWithImages() / (decimal)_dataProvider.TotalNumberOfTweets() * 100);
                    }
                }

                if (hashtags != null)
                {
                    foreach (var hashtag in hashtags)
                    {
                        _dataProvider.AddHashtag(hashtag);
                    }
                }

                var emojis = _dataProvider.EmojiLibrary();

                if (emojis == null)
                {
                    _dataProvider.AddError("Emoji lookup has not been loaded.");
                }
                else
                {
                    foreach (var emoji in emojis.Where(r => int.TryParse(r.Unified, System.Globalization.NumberStyles.HexNumber, null, out _)))
                    {
                        int value = int.Parse(emoji.Unified, System.Globalization.NumberStyles.HexNumber);
                        string result = char.ConvertFromUtf32(value).ToString();
                        if (Regex.IsMatch(text, result))
                        {
                            _dataProvider.AddTweetWithEmoji();
                            _dataProvider.AddEmoji(emoji.Short_name);
                        }
                    }
                    _dataProvider.SetPercentOfTweetsWithEmojis(_dataProvider.NumberOfTweetsWithEmojis() / (decimal)_dataProvider.TotalNumberOfTweets() * 100);
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
