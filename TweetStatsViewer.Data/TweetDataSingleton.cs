using System;
using System.Collections.Generic;
using TweetStatsViewer.Models;

namespace TweetStatsViewer.Data
{
    public sealed class TweetDataSingleton
    {
        public DateTime InstanceCreatedAtUtc { get; set; }

        public int AverageTweetsPerHour { get; set; }

        public int AverageTweetsPerMinute { get; set; }

        public int AverageTweetsPerSecond { get; set; }

        public int TotalNumberOfTweets { get; set; }

        public int NumberOfTweetsWithEmojis { get; set; }

        public int PercentOfTweetsWithEmojis { get; set; }

        public int NumberOfTweetsWithUrls { get; set; }

        public int PercentOfTweetsWithUrls { get; set; }

        public int NumberOfTweetsWithImageUrls { get; set; }

        public int PercentOfTweetsWithImageUrls { get; set; }

        public ICollection<Emoji> EmojiLibrary { get; set; }

        public IDictionary<string, int> TopEmojis { get; set; }

        public IDictionary<string, int> TopDomains { get; set; }

        public IDictionary<string, int> TopHashtags { get; set; }

        public ICollection<string> Errors { get; set; }

        private static readonly TweetDataSingleton instance = new TweetDataSingleton();

        public static TweetDataSingleton Instance
        {
            get
            {
                return instance;
            }
        }

        static TweetDataSingleton()
        {
        }

        private TweetDataSingleton()
        {
            InstanceCreatedAtUtc = DateTime.UtcNow;
            TopEmojis = new Dictionary<string, int>();
            TopDomains = new Dictionary<string, int>();
            TopHashtags = new Dictionary<string, int>();
            Errors = new List<string>();
        }
    }
}
