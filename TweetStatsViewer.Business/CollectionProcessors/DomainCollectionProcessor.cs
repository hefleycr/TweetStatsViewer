using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TweetStatsViewer.Interfaces;

namespace TweetStatsViewer.Business.CollectionProcessors
{
    public class DomainCollectionProcessor : ICollectionProcessor
    {
        private readonly ITweetDataProvider _dataProvider;
        private readonly string _domainRegex = @"https?:\/\/(www\.)?([-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-z]{2,4})\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)";
        private readonly string[] _imageUrls = { "pic.twitter.com", "instagram.com" };

        public DomainCollectionProcessor(ITweetDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public void ProcessTweet(string text, IEnumerable<string> urls, IEnumerable<string> hashtags)
        {
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
        }
    }
}
