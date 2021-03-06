using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using TweetStatsViewer.Business;
using TweetStatsViewer.Interfaces;
using TweetStatsViewer.Models;

namespace TweetStatsViewer.Tests
{
    [TestClass]
    public class ReceivedTweetProcessorTests
    {
        private ReceivedTweetProcessor _underTest;
        private Mock<ITweetDataProvider> _mockDataProvider = new Mock<ITweetDataProvider>();
        private Mock<IEnumerable<ICollectionProcessor>> _mockCollectionProcessor = new Mock<IEnumerable<ICollectionProcessor>>();
        private readonly string _unified_value = "12345";

        [TestInitialize]
        public void Initialize()
        {
            _mockDataProvider.Setup(r => r.EmojiLibrary()).Returns(new List<Emoji>() { new Emoji { Unified = _unified_value, Short_name = "smiley" } });
            _mockDataProvider.Setup(r => r.TotalNumberOfTweets()).Returns(1);
            _mockDataProvider.Setup(r => r.NumberOfTweetsWithEmojis()).Returns(1);
            _mockDataProvider.Setup(r => r.NumberOfTweetsWithUrls()).Returns(1);
            _mockDataProvider.Setup(r => r.NumberOfTweetsWithImages()).Returns(1);
        }

        [TestMethod]
        public void GivenReceivingFirstTweetBeforeLoadingEmojiLibrary_LogsError()
        {
            //Arrange
            ICollection<Emoji> lib = null;
            _mockDataProvider.Setup(r => r.EmojiLibrary()).Returns(lib);
            _underTest = new ReceivedTweetProcessor(_mockDataProvider.Object, _mockCollectionProcessor.Object);

            //Act
            _underTest.ProcessTweet("Test tweet message text.", null, null);

            //Assert
            _mockDataProvider.Verify(r => r.AddError(It.IsAny<string>()), Times.Once());
        }
    }
}
