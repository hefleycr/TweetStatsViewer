using System;
using System.Collections.Generic;
using TweetStatsViewer.Data;
using TweetStatsViewer.Interfaces;
using TweetStatsViewer.Models;

namespace TweetStatsViewer.Business
{
    public class TweetDataProvider : ITweetDataProvider
    {
        private readonly TweetDataSingleton _tweetData;
        private readonly IJsonDeserializer _jsonDeserializer;
        private readonly IFileReader _fileReader;

        public TweetDataProvider(IJsonDeserializer jsonDeserializer, IFileReader fileReader)
        {
            _jsonDeserializer = jsonDeserializer;
            _fileReader = fileReader;
            _tweetData = TweetDataSingleton.Instance;
        }

        public int TotalNumberOfTweets() => _tweetData.TotalNumberOfTweets;
        public int AverageTweetsPerHour() => _tweetData.AverageTweetsPerHour;
        public int AverageTweetsPerMinute() => _tweetData.AverageTweetsPerMinute;
        public int AverageTweetsPerSecond() => _tweetData.AverageTweetsPerSecond;
        public int PercentOfTweetsWithEmojis() => _tweetData.PercentOfTweetsWithEmojis;
        public int PercentOfTweetsWithImages() => _tweetData.PercentOfTweetsWithImageUrls;
        public int PercentOfTweetsWithUrls() => _tweetData.PercentOfTweetsWithUrls;
        public int NumberOfTweetsWithEmojis() => _tweetData.NumberOfTweetsWithEmojis;
        public int NumberOfTweetsWithImages() => _tweetData.NumberOfTweetsWithImageUrls;
        public int NumberOfTweetsWithUrls() => _tweetData.NumberOfTweetsWithUrls;
        public DateTime InstanceCreatedAtUtc() => _tweetData.InstanceCreatedAtUtc;
        public ICollection<Emoji> EmojiLibrary() => _tweetData.EmojiLibrary;
        public void AddTweet() => _tweetData.TotalNumberOfTweets++;
        public void AddTweetWithEmoji() => _tweetData.NumberOfTweetsWithEmojis++;
        public void AddTweetWithUrl() => _tweetData.NumberOfTweetsWithUrls++;
        public void AddTweetWithImage() => _tweetData.NumberOfTweetsWithImageUrls++;
        public void AddEmoji(string value) => AddEntry(_tweetData.TopEmojis, value);
        public void AddDomain(string value) => AddEntry(_tweetData.TopDomains, value);
        public void AddHashtag(string value) => AddEntry(_tweetData.TopHashtags, value);
        public Dictionary<string, int> GetTopEmojisForDisplay() => new Dictionary<string, int>(_tweetData.TopEmojis);
        public Dictionary<string, int> GetTopDomainsForDisplay() => new Dictionary<string, int>(_tweetData.TopDomains);
        public Dictionary<string, int> GetTopHashtagsForDisplay() => new Dictionary<string, int>(_tweetData.TopHashtags);
        public List<string> GetErrorsForDisplay() => new List<string>(_tweetData.Errors);
        public void AddError(string value) => _tweetData.Errors.Add(value);
        public void SetPercentOfTweetsWithEmojis(int num) => _tweetData.PercentOfTweetsWithEmojis = num;
        public void SetPercentOfTweetsWithUrls(int num) => _tweetData.PercentOfTweetsWithUrls = num;
        public void SetPercentOfTweetsWithImages(int num) => _tweetData.PercentOfTweetsWithImageUrls = num;
        public void SetAverageTweetsPerHour(int num) => _tweetData.AverageTweetsPerHour = num;
        public void SetAverageTweetsPerMinute(int num) => _tweetData.AverageTweetsPerMinute = num;
        public void SetAverageTweetsPerSecond(int num) => _tweetData.AverageTweetsPerSecond = num;

        public void LoadEmojiLibraryFromFile(string fileName)
        {
            _tweetData.EmojiLibrary = _jsonDeserializer.DeserializeEmojisCollection(_fileReader.GetEmojisFromFile(fileName));
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
