using System.Collections.Generic;
using TweetStatsViewer.Data;

namespace TweetStatsViewer.Interfaces
{
    public interface ITweetDataProvider
    {
        TweetDataSingleton GetData();

        Dictionary<string, int> GetTopEmojisForDisplay();

        Dictionary<string, int> GetTopDomainsForDisplay();

        Dictionary<string, int> GetTopHashtagsForDisplay();

        List<string> GetErrorsForDisplay();

        void LoadEmojiLibraryFromFile(string fileName);
    }
}
