using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using TweetStatsViewer.Business.CollectionProcessors;
using TweetStatsViewer.Interfaces;
using TweetStatsViewer.Models;

namespace TweetStatsViewer.Tests
{
    [TestClass]
    public class DomainCollectionProcessorTests
    {
        private DomainCollectionProcessor _underTest;
        private Mock<ITweetDataProvider> _mockDataProvider = new Mock<ITweetDataProvider>();
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
        public void GivenReceivingTwoTweetsWithUrls_SavesDomains()
        {
            //Arrange
            _underTest = new DomainCollectionProcessor(_mockDataProvider.Object);

            //Act
            _underTest.ProcessTweet("Test tweet message text.", new string[] { "https://jha.com" }, null);
            _underTest.ProcessTweet("Test tweet message text.", new string[] { "https://instagram.com" }, null);

            //Assert
            _mockDataProvider.Verify(r => r.AddDomain(It.IsAny<string>()), Times.Exactly(2));
            _mockDataProvider.Verify(r => r.AddHashtag(It.IsAny<string>()), Times.Never());
            _mockDataProvider.Verify(r => r.SetPercentOfTweetsWithImages(It.IsAny<decimal>()), Times.Once());
            _mockDataProvider.Verify(r => r.SetPercentOfTweetsWithUrls(It.IsAny<decimal>()), Times.Exactly(2));
        }
    }
}
