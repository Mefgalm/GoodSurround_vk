using GoodSurround.Logic.Vk;
using GoodSurround.Logic.Response;
using System;
using System.Web.Http;
using System.Collections.Generic;

namespace GoodSurround.ApiControllers
{
    [RoutePrefix("api/v1/audio")]
    public class AudioController : ApiController
    {
        public readonly AuthService _vkAuthService;
        public readonly AudioService _vkMusicService;

        public AudioController()
        {
            _vkAuthService = new AuthService();
            _vkMusicService = new AudioService();
        }

        [HttpPost, Route("loadMusic/{token}")]
        public ApiResponse<object> LoadMusic(Guid token)
        {
            ApiResponse<DataModels.User> response = _vkAuthService.CheckToken(token);
            if(!response.Ok)            
                return new ApiResponse<object>(response.ErrorMessage);

            return _vkMusicService.LoadMusic(response.Data);
        }

        [HttpGet, Route("scheduleAudios")]
        public ApiResponse<IEnumerable<ApiModels.Audio>> GetAudios(Guid token, int scheduleId, int skip, int take)
        {
            ApiResponse<DataModels.User> response = _vkAuthService.CheckToken(token);
            if (!response.Ok)
                return new ApiResponse<IEnumerable<ApiModels.Audio>>(response.ErrorMessage);

            return _vkMusicService.GetAudios(response.Data, scheduleId, skip, take);
        }

        [HttpGet, Route("audios")]
        public ApiResponse<IEnumerable<ApiModels.Audio>> GetAudios(Guid token, int skip, int take)
        {
            ApiResponse<DataModels.User> response = _vkAuthService.CheckToken(token);
            if (!response.Ok)
                return new ApiResponse<IEnumerable<ApiModels.Audio>>(response.ErrorMessage);

            return _vkMusicService.GetAudios(response.Data, skip, take);
        }

        protected override void Dispose(bool disposing)
        {
            _vkAuthService.Dispose();
            _vkMusicService.Dispose();
            base.Dispose(disposing);
        }
    }
}
