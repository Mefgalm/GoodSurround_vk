using System.Collections.Generic;

namespace GoodSurround.ApiModels.Messages
{
    public class AudioRequest
    {
        public Dictionary<int, int> Users { get; set; }
        public List<int> ExcludeAudios { get; set; }
    }

    
}
