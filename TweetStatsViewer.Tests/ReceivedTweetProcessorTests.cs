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
        private readonly string _unified_value = "12345";

        [TestMethod]
        public void GivenReceivingTwoTweetsWithEmojis_SavesEmojis()
        {
            //Arrange
            _mockDataProvider.Setup(r => r.EmojiLibrary()).Returns(new List<Models.Emoji>() { new Models.Emoji { Unified = _unified_value, Short_name = "smiley" } });
            _underTest = new ReceivedTweetProcessor(_mockDataProvider.Object);

            //Act
            _underTest.ProcessTweet("Test tweet message text \U00012345.", null, null);
            _underTest.ProcessTweet("Test tweet message text \U00012345.", null, null);

            //Assert
            _mockDataProvider.Verify(r => r.AddEmoji(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void GivenReceivingFirstTweetBeforeLoadingEmojiLibrary_LogsError()
        {
            //Arrange
            ICollection<Emoji> lib = null;
            _mockDataProvider.Setup(r => r.EmojiLibrary()).Returns(lib);
            _underTest = new ReceivedTweetProcessor(_mockDataProvider.Object);

            //Act
            _underTest.ProcessTweet("Test tweet message text.", null, null);

            //Assert
            _mockDataProvider.Verify(r => r.AddError(It.IsAny<string>()), Times.Once());
        }
    }
}
