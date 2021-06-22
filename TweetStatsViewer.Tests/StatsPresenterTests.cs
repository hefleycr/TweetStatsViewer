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
        private Mock<ITweetDataProvider> _mockDataProvider = new Mock<ITweetDataProvider>();
        private Mock<IDisplayHandler> _mockDisplayHandler = new Mock<IDisplayHandler>();

        [TestMethod]
        public void GivenTweetCountOne_WritingMultipleLinesToOutput()
        {
            //Arrange
            var data = TweetDataSingleton.Instance;
            data.NumberOfTweetsReceived = 1;
            _mockDataProvider.Setup(r => r.GetData()).Returns(data);
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
            var data = TweetDataSingleton.Instance;
            data.NumberOfTweetsReceived = 0;
            data.Errors = new List<string>();
            _mockDataProvider.Setup(r => r.GetData()).Returns(data);
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
            var data = TweetDataSingleton.Instance;
            data.NumberOfTweetsReceived = 0;
            data.Errors = new List<string>() { "Error" };
            _mockDataProvider.Setup(r => r.GetData()).Returns(data);
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
            var data = TweetDataSingleton.Instance;
            data.NumberOfTweetsReceived = 1;
            data.EmojiCounts = new Dictionary<string, int> { { "smiley", 1 } };
            _mockDataProvider.Setup(r => r.GetData()).Returns(data);
            _underTest = new StatsPresenter(_mockDataProvider.Object, _mockDisplayHandler.Object);

            //Act
            _underTest.Present();

            //Assert
            _mockDisplayHandler.Verify(mock => mock.WriteLine(It.IsRegex(".*Top.*Emojis.*")), Times.Once());
        }
    }
}
