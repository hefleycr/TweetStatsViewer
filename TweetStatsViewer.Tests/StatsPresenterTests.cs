using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using TweetStatsViewer.Business;
using TweetStatsViewer.Data;
using TweetStatsViewer.Interfaces;

namespace TweetStatsViewer.Tests
{
    [TestClass]
    public class StatsPresenterTests
    {
        private StatsPresenter _underTest;
        private readonly Mock<ITweetDataProvider> _mockDataProvider = new Mock<ITweetDataProvider>();
        private readonly Mock<IDisplayHandler> _mockDisplayHandler = new Mock<IDisplayHandler>();

        [TestInitialize]
        public void Initialize()
        {
            TweetDataSingleton.Instance.TotalNumberOfTweets = 1;
            _mockDataProvider.Setup(r => r.GetData()).Returns(TweetDataSingleton.Instance);
            _mockDataProvider.Setup(r => r.GetTopEmojisForDisplay()).Returns(new Dictionary<string, int>());
            _mockDataProvider.Setup(r => r.GetTopDomainsForDisplay()).Returns(new Dictionary<string, int>());
            _mockDataProvider.Setup(r => r.GetTopHashtagsForDisplay()).Returns(new Dictionary<string, int>());
            _mockDataProvider.Setup(r => r.GetErrorsForDisplay()).Returns(new List<string>());
        }

        [TestMethod]
        public void GivenTweetCountOne_WritingMultipleLinesToOutput()
        {
            //Arrange
            _underTest = new StatsPresenter(_mockDataProvider.Object, _mockDisplayHandler.Object);

            //Act
            _underTest.Present();

            //Assert
            _mockDisplayHandler.Verify(mock => mock.WriteLine(It.IsAny<string>()), Times.AtLeast(2));
        }

        [TestMethod]
        public void GivenTweetCountZeroNoErrors_WritingOneLineToOutput()
        {
            //Arrange
            TweetDataSingleton.Instance.TotalNumberOfTweets = 0;
            _underTest = new StatsPresenter(_mockDataProvider.Object, _mockDisplayHandler.Object);

            //Act
            _underTest.Present();

            //Assert
            _mockDisplayHandler.Verify(mock => mock.WriteLine(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void GivenTweetCountZeroOneError_WritingTwoLinesToOutput()
        {
            //Arrange
            TweetDataSingleton.Instance.TotalNumberOfTweets = 0;
            _mockDataProvider.Setup(r => r.GetErrorsForDisplay()).Returns(new List<string>() { { "Error" } });
            _underTest = new StatsPresenter(_mockDataProvider.Object, _mockDisplayHandler.Object);

            //Act
            _underTest.Present();

            //Assert
            _mockDisplayHandler.Verify(mock => mock.WriteLine(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void GivenEmojiCountOne_WritingEmojiTable()
        {
            //Arrange
            _mockDataProvider.Setup(r => r.GetTopEmojisForDisplay()).Returns(new Dictionary<string, int>() { { "smiley", 1 } });
            _underTest = new StatsPresenter(_mockDataProvider.Object, _mockDisplayHandler.Object);

            //Act
            _underTest.Present();

            //Assert
            _mockDisplayHandler.Verify(mock => mock.WriteLine(It.IsRegex(".*Top.*Emojis.*")), Times.Once());
        }

        [TestMethod]
        public void GivenDomainCountOne_WritingDomainsTable()
        {
            //Arrange
            _mockDataProvider.Setup(r => r.GetTopDomainsForDisplay()).Returns(new Dictionary<string, int>() { { "jha.com", 1 } });
            _underTest = new StatsPresenter(_mockDataProvider.Object, _mockDisplayHandler.Object);

            //Act
            _underTest.Present();

            //Assert
            _mockDisplayHandler.Verify(mock => mock.WriteLine(It.IsRegex(".*Top.*Domains.*")), Times.Once());
        }

        [TestMethod]
        public void GivenHashtagCountOne_WritingHashtagsTable()
        {
            //Arrange
            _mockDataProvider.Setup(r => r.GetTopHashtagsForDisplay()).Returns(new Dictionary<string, int>() { { "JHARocks", 1 } });
            _underTest = new StatsPresenter(_mockDataProvider.Object, _mockDisplayHandler.Object);

            //Act
            _underTest.Present();

            //Assert
            _mockDisplayHandler.Verify(mock => mock.WriteLine(It.IsRegex(".*Top.*Hashtags.*")), Times.Once());
        }
    }
}
