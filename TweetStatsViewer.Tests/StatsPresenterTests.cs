using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
        public void GivenTweetCountZero_WritingOneLineToOutput()
        {
            //Arrange
            var data = TweetDataSingleton.Instance;
            data.NumberOfTweetsReceived = 0;
            _mockDataProvider.Setup(r => r.GetData()).Returns(data);
            _underTest = new StatsPresenter(_mockDataProvider.Object, _mockDisplayHandler.Object);

            //Act
            _underTest.Present();

            //Assert
            _mockDisplayHandler.Verify(mock => mock.WriteLine(It.IsAny<string>()), Times.Once());
        }
    }
}
