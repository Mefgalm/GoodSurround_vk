using System;

namespace GoodSurround.ApiModels
{
    public class Audio
    {
        public int Id { get; set; }

        public int VkId { get; set; }

        public string Artist { get; set; }

        public string Title { get; set; }

        public int Duration { get; set; }

        public DateTime Date { get; set; }

        public Album Album { get; set; }

        public User User { get; set; }

        public string Url { get; set; }
    }
}
