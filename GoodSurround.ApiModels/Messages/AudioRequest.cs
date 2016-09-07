using System.Collections.Generic;

namespace GoodSurround.ApiModels.Messages
{
    public class AudioRequest
    {
        public List<UserInfo> Users { get; set; }
        public List<int> ExcludeAudios { get; set; }
    }

    public class UserInfo
    {
        public int UserId { get; set; }
        public int AudioCount { get; set; }
        public bool IsAudioMixes { get; set; }
    }
}
