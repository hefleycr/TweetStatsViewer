using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TweetStatsViewer.Interfaces;

namespace TweetStatsViewer.Business.CollectionProcessors
{
    public class EmojiCollectionProcessor : ICollectionProcessor
    {
        private readonly ITweetDataProvider _dataProvider;

        public EmojiCollectionProcessor(ITweetDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public void ProcessTweet(string text, IEnumerable<string> urls, IEnumerable<string> hashtags)
        {
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
    }
}
