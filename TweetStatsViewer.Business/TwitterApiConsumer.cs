using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Events.V2;
using Tweetinvi.Models;
using Tweetinvi.Streaming.V2;
using TweetStatsViewer.Interfaces;

namespace TweetStatsViewer.Business
{
    public class TwitterApiConsumer : ITwitterApiConsumer
    {
        private ISampleStreamV2 _sampleStream;
        private readonly IReceivedTweetProcessor _tweetProcessor;
        private readonly IConfigurationLoader _configurationLoader;

        public TwitterApiConsumer(IReceivedTweetProcessor tweetProcessor, IConfigurationLoader configurationLoader)
        {
            _tweetProcessor = tweetProcessor;
            _configurationLoader = configurationLoader;
        }

        public void StartStream()
        {
            var credentials = _configurationLoader.GetTwitterApiCredentials();
            var appClient = new TwitterClient(new ConsumerOnlyCredentials(credentials.ApiKey, credentials.ApiSecret, credentials.BearerToken));

            _sampleStream = appClient.StreamsV2.CreateSampleStream();
            _sampleStream.TweetReceived += HandleTweetReceived;

            _sampleStream.StartAsync();
        }

        public void StopStream()
        {
            if (_sampleStream != null)
            {
                _sampleStream.StopStream();
            }
        }

        private void HandleTweetReceived(object sender, TweetV2ReceivedEventArgs args)
        {
            _tweetProcessor.ProcessTweet(args.Tweet.Text);
        }
    }
}
