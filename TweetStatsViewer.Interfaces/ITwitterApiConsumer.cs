using TweetStatsViewer.Models;

namespace TweetStatsViewer.Interfaces
{
    public interface ITwitterApiConsumer
    {
        void StartStream();

        void StopStream();
    }
}
