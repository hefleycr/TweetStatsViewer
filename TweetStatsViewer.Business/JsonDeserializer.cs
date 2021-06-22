using Newtonsoft.Json;
using System.Collections.Generic;
using TweetStatsViewer.Interfaces;
using TweetStatsViewer.Models;

namespace TweetStatsViewer.Business
{
    public class JsonDeserializer : IJsonDeserializer
    {
        public ICollection<Emoji> DeserializeEmojisCollection(string collection)
        {
            return JsonConvert.DeserializeObject<List<Emoji>>(collection);
        }
    }
}
