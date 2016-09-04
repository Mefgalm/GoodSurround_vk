using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodSurround.Logic.Mappers
{
    internal sealed class VkMapper
    {
        public static DataModels.Album GetAlbum(VkModels.Album album)
        {
            return new DataModels.Album()
            {
                VkId = album.id,
                Title = album.title,
            };
        }

        public static DataModels.Audio GetAudio(VkModels.Audio audio)
        {
            return new DataModels.Audio()
            {
                VkId = audio.id,
                Artist = audio.artist,
                Date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                            .AddMilliseconds(audio.date * 1000)
                            .ToLocalTime(),
                Title = audio.title,
                Duration = audio.duration,    
                AlbumId = audio.album_id,                            
            };
        }
    }
}
