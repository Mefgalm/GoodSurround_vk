using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodSurround.VkModels
{
    public class Audio
    {
        public int id { get; set; }
        public string artist { get; set; }
        public string title { get; set; }
        public int duration { get; set; }
        public long date { get; set; }
        public int? album_id { get; set; }
        public string url { get; set; }
        //TODO: do I need this?
        //public int genre_id { get;set;}
    }

    public class AudioEtity
    {
        public AudioResponse response { get; set; }
        public Error error { get; set; }
    }

    public class AudioResponse
    {
        public int count { get; set; }
        public Audio[] items { get; set; }
    }

    public class AudioById
    {
        public Audio[] response { get; set; }
    }

}
