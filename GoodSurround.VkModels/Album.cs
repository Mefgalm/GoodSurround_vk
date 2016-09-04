using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodSurround.VkModels
{
    public class Album
    {
        public int id { get; set; }
        public string title { get; set; }
    }

    public class AlbumEtity
    {
        public AlbumResponse response { get; set; }
        public Error error { get; set; }
    }

    public class AlbumResponse
    {
        public int count { get; set; }
        public Album[] items { get; set; }
    }
}
