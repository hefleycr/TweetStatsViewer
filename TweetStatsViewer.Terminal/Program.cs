using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TweetStatsViewer.Business;
using TweetStatsViewer.Business.CollectionProcessors;
using TweetStatsViewer.Interfaces;

namespace TweetStatsViewer.Terminal
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ITwitterApiConsumer, TwitterApiConsumer>()
                .AddSingleton<IFileReader, FileReader>()
                .AddSingleton<IJsonDeserializer, JsonDeserializer>()
                .AddSingleton<IStatsPresenter, StatsPresenter>()
                .AddSingleton<ITweetDataProvider, TweetDataProvider>()
                .AddSingleton<IConfigurationLoader, ConfigurationLoader>()
                .AddSingleton<IReceivedTweetProcessor, ReceivedTweetProcessor>()
                .AddSingleton<IDisplayHandler, DisplayHandler>()
                .AddSingleton<ICollectionProcessor, EmojiCollectionProcessor>()
                .AddSingleton<ICollectionProcessor, DomainCollectionProcessor>()
                .AddSingleton<ICollectionProcessor, HashtagCollectionProcessor>()
                .BuildServiceProvider();

            var apiConsumer = serviceProvider.GetService<ITwitterApiConsumer>();
            var statsPresenter = serviceProvider.GetService<IStatsPresenter>();
            var tweetDataProvider = serviceProvider.GetService<ITweetDataProvider>();

            tweetDataProvider.LoadEmojiLibraryFromFile("emoji.json");

            apiConsumer.StartStream();

            Task task = Task.Factory.StartNew(() => statsPresenter.PresentationLoop());

            Console.Read();

            apiConsumer.StopStream();
        }
    }
}
