using System.Collections.Generic;
using TweetStatsViewer.Models;

namespace TweetStatsViewer.Interfaces
{
    public interface IJsonDeserializer
    {
        ICollection<Emoji> DeserializeEmojisCollection(string collection);
    }
}
