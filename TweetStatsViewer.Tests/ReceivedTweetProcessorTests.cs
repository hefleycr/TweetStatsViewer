using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using TweetStatsViewer.Business;
using TweetStatsViewer.Data;
using TweetStatsViewer.Interfaces;

namespace TweetStatsViewer.Tests
{
    [TestClass]
    public class ReceivedTweetProcessorTests
    {
        private ReceivedTweetProcessor _underTest;
        private Mock<ITweetDataProvider> _mockDataProvider = new Mock<ITweetDataProvider>();
        private readonly string _unified_value = "12345";

        [TestMethod]
        public void GivenReceivingFirstAndSecondTweets_SavesStatisticsToData()
        {
            //Arrange
            var data = TweetDataSingleton.Instance;
            data.EmojiLibrary = new List<Models.Emoji>() { new Models.Emoji { Unified = _unified_value, Short_name = "smiley" } };
            _mockDataProvider.Setup(r => r.GetData()).Returns(data);
            _underTest = new ReceivedTweetProcessor(_mockDataProvider.Object);

            //Act
            _underTest.ProcessTweet("Test tweet message text \U00012345.", null, null);
            _underTest.ProcessTweet("Test tweet message text \U00012345.", null, null);

            //Assert
            Assert.IsTrue(data.TopEmojis.Count > 0 && data.TopEmojis["smiley"] == 2);
        }

        [TestMethod]
        public void GivenReceivingFirstTweetBeforeLoadingEmojiLibrary_LogsError()
        {
            //Arrange
            var data = TweetDataSingleton.Instance;
            data.EmojiLibrary = null;
            _mockDataProvider.Setup(r => r.GetData()).Returns(data);
            _underTest = new ReceivedTweetProcessor(_mockDataProvider.Object);

            //Act
            _underTest.ProcessTweet("Test tweet message text.", null, null);

            //Assert
            Assert.IsTrue(data.Errors.Count > 0 && data.Errors.First() == "Emoji lookup has not been loaded.");
        }
    }
}
