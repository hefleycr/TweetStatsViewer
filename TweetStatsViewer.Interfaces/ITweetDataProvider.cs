using TweetStatsViewer.Data;

namespace TweetStatsViewer.Interfaces
{
    public interface ITweetDataProvider
    {
        TweetDataSingleton GetData();

        void LoadEmojiLibraryFromFile(string fileName);
    }
}
