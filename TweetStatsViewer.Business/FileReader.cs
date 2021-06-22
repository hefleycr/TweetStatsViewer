using System.IO;
using TweetStatsViewer.Interfaces;

namespace TweetStatsViewer.Business
{
    public class FileReader : IFileReader
    {
        public string GetEmojisFromFile(string fileName)
        {
            return File.ReadAllText(fileName);
        }
    }
}
