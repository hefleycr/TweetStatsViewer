using System;
using System.Collections.Generic;
using TweetStatsViewer.Models;

namespace TweetStatsViewer.Interfaces
{
    public interface ITweetDataProvider
    {
        Dictionary<string, int> GetTopEmojisForDisplay();

        Dictionary<string, int> GetTopDomainsForDisplay();

        Dictionary<string, int> GetTopHashtagsForDisplay();

        List<string> GetErrorsForDisplay();

        ICollection<Emoji> EmojiLibrary();

        void LoadEmojiLibraryFromFile(string fileName);

        DateTime InstanceCreatedAtUtc();

        int TotalNumberOfTweets();

        int AverageTweetsPerHour();

        int AverageTweetsPerMinute();

        int AverageTweetsPerSecond();

        int PercentOfTweetsWithEmojis();

        int PercentOfTweetsWithImages();

        int PercentOfTweetsWithUrls();

        int NumberOfTweetsWithEmojis();

        int NumberOfTweetsWithImages();

        int NumberOfTweetsWithUrls();

        void AddTweet();

        void AddTweetWithEmoji();

        void AddTweetWithUrl();

        void AddTweetWithImage();

        void AddEmoji(string value);

        void AddDomain(string value);

        void AddHashtag(string value);

        void AddError(string value);

        void SetPercentOfTweetsWithEmojis(int num);

        void SetPercentOfTweetsWithUrls(int num);

        void SetPercentOfTweetsWithImages(int num);

        void SetAverageTweetsPerHour(int num);

        void SetAverageTweetsPerMinute(int num);

        void SetAverageTweetsPerSecond(int num);
    }
}
