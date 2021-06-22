using Microsoft.VisualStudio.TestTools.UnitTesting;
using TweetStatsViewer.Business;

namespace TweetStatsViewer.Tests
{
    [TestClass]
    public class ConfigurationLoaderTests
    {
        private ConfigurationLoader _underTest;

        [TestMethod]
        public void GivenTestAppsettings_ReadsFileAndLoadsApiKey()
        {
            //Arrange
            _underTest = new ConfigurationLoader();

            //Act
            var result = _underTest.GetTwitterApiCredentials();

            //Assert
            result.ApiKey = "mock-api-key";
        }
    }
}
