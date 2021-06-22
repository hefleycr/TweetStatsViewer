using System;
using TweetStatsViewer.Interfaces;

namespace TweetStatsViewer.Business
{
    public class DisplayHandler : IDisplayHandler
    {
        public void WriteLine(string line)
        {
            Console.WriteLine(line);
        }

        public void Clear()
        {
            Console.Clear();
        }
    }
}
