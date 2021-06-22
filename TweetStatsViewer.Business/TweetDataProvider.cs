using System.Collections.Generic;
using TweetStatsViewer.Data;
using TweetStatsViewer.Interfaces;

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

        public TweetDataSingleton GetData()
        {
            return _tweetData;
        }

        public Dictionary<string, int> GetTopEmojisForDisplay()
        {
            return new Dictionary<string, int>(_tweetData.TopEmojis);
        }

        public Dictionary<string, int> GetTopDomainsForDisplay()
        {
            return new Dictionary<string, int>(_tweetData.TopDomains);
        }

        public Dictionary<string, int> GetTopHashtagsForDisplay()
        {
            return new Dictionary<string, int>(_tweetData.TopHashtags);
        }

        public List<string> GetErrorsForDisplay()
        {
            return new List<string>(_tweetData.Errors);
        }

        public void LoadEmojiLibraryFromFile(string fileName)
        {
            _tweetData.EmojiLibrary = _jsonDeserializer.DeserializeEmojisCollection(_fileReader.GetEmojisFromFile(fileName));
        }
    }
}
