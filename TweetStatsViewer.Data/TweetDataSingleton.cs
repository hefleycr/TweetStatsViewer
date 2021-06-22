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

        public int NumberOfTweetsReceived { get; set; }

        public int NumberOfTweetsWithEmojisReceived { get; set; }

        public Dictionary<string, int> EmojiCounts { get; set; }

        public ICollection<Emoji> EmojiLibrary { get; set; }

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
            EmojiCounts = new Dictionary<string, int>();
        }
    }
}
