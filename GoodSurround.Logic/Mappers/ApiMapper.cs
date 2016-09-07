namespace GoodSurround.Logic.Mappers
{
    public static class ApiMapper
    {
        public static ApiModels.Audio GetAudio(DataModels.Audio audio)
        {
            return new ApiModels.Audio()
            {
                Id = audio.Id,
                VkId = audio.VkId,
                Album = GetAlbum(audio.Album),
                Artist = audio.Artist,
                Date = audio.Date,
                Duration = audio.Duration,
                Title = audio.Title,
                User = GetUser(audio.User),
            };
        }

        public static ApiModels.Album GetAlbum(DataModels.Album album)
        {
            if (album == null)
                return null;

            return new ApiModels.Album()
            {
                Id = album.VkId,
                Title = album.Title,
            };
        }

        public static ApiModels.User GetUser(DataModels.User user)
        {
            return new ApiModels.User()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Photo50 = user.Photo50,
            };
        }

        public static ApiModels.AuthUser GetAuthUser(DataModels.User user)
        {
            return new ApiModels.AuthUser()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Photo50 = user.Photo50,
                Token = user.Token,
            };
        }
    }
}
