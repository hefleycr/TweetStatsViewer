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

        public void LoadEmojiLibraryFromFile(string fileName)
        {
            _tweetData.EmojiLibrary = _jsonDeserializer.DeserializeEmojisCollection(_fileReader.GetEmojisFromFile(fileName));
        }
    }
}
