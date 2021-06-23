using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using TweetStatsViewer.Business;
using TweetStatsViewer.Interfaces;

namespace TweetStatsViewer.Tests
{
    [TestClass]
    public class TweetDataProviderTests
    {
        private TweetDataProvider _underTest;
        private Mock<IJsonDeserializer> _mockJsonDeserializer = new Mock<IJsonDeserializer>();
        private Mock<IFileReader> _mockFileReader = new Mock<IFileReader>();
        private readonly string _unified_value = "123";

        [TestMethod]
        public void GivenOneEmojiInList_ReturnsEmojiInResult()
        {
            //Arrange
            var emojiList = new List<Models.Emoji>()
            {
                new Models.Emoji
                {
                    Unified = _unified_value
                }
            };
            _mockJsonDeserializer.Setup(r => r.DeserializeEmojisCollection(It.IsAny<string>())).Returns(emojiList);
            _underTest = new TweetDataProvider(_mockJsonDeserializer.Object, _mockFileReader.Object);

            //Act
            _underTest.LoadEmojiLibraryFromFile(string.Empty);

            //Assert
            _mockFileReader.Verify(mock => mock.GetEmojisFromFile(It.IsAny<string>()), Times.Once());
            Assert.IsTrue(_underTest.EmojiLibrary().FirstOrDefault()?.Unified == _unified_value);
        }
    }
}
