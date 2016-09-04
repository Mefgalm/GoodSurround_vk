using GoodSurround.VkModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using NLog;

namespace GoodSurround.Logic.Vk
{
    internal class VkWebService
    {
        private readonly int VkAppId = int.Parse(ConfigurationManager.AppSettings["VkAppId"]);
        private readonly string VkAppSercret = ConfigurationManager.AppSettings["VkAppSercret"];
        private readonly string VkRedirectUri = ConfigurationManager.AppSettings["VkRedirectUri"];
        private readonly string VkApiVersion = ConfigurationManager.AppSettings["VkApiVersion"];        

        private T DownloadObject<T>(string url)
        {
            string jsonResponse = null;

            using (var webClient = new WebClient())
            {
                jsonResponse = webClient.DownloadString(url);
            }

            byte[] cp1252String = Encoding.GetEncoding(1252).GetBytes(jsonResponse);

            jsonResponse = Encoding.UTF8.GetString(cp1252String);

            if (string.IsNullOrWhiteSpace(jsonResponse))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(jsonResponse);
        }



        public AccessToken GetAccessToken(string code)
        {
            string url =
                "https://oauth.vk.com/access_token?" +
                    $"client_id={VkAppId}" +
                    $"&client_secret={VkAppSercret}" +
                    $"&redirect_uri={VkRedirectUri}" +
                    $"&code={code}";

            return DownloadObject<AccessToken>(url);
        }



        public UserEntity GetUserEntity(string accessToken, int userId)
        {
            string url =
                "https://api.vk.com/method/users.get?" +
                   $"user_ids={userId}&" +
                    "fields=photo_50&" +
                    $"access_token={accessToken}&" +
                    $"v={VkApiVersion}";

            return DownloadObject<UserEntity>(url);
        }

        public AlbumEtity GetAlbums(string accessToken, int count)
        {
            string url =
                "https://api.vk.com/method/audio.getAlbums?" +
                    "offset=0&" +
                    $"count={count}&" +
                    $"access_token={accessToken}&" +
                    $"v={VkApiVersion}";

            return DownloadObject<AlbumEtity>(url);
        }

        public AudioById GetAudio(string accessToken, int userId, int vkAudionId)
        {
            string url =
                "https://api.vk.com/method/audio.getById?" +
                    $"audios={userId}_{vkAudionId}&" +
                    $"access_token={accessToken}&" +
                    $"v={VkApiVersion}";

            return DownloadObject<AudioById>(url);
        }

        public AudioEtity GetAudios(string accessToken, int count)
        {
            string url =
                 "https://api.vk.com/method/audio.get?" +
                    "offset=0&" +
                    $"count={count}&" +
                    $"access_token={accessToken}&" +
                    $"v={VkApiVersion}";

            return DownloadObject<AudioEtity>(url);
        }
    }
}
