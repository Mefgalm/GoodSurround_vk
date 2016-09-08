using GoodSurround.Logic.Vk;
using GoodSurround.Logic.Response;
using System;
using System.Web.Http;
using System.Collections.Generic;
using GoodSurround.ApiModels.Messages;

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

        [HttpPost, Route("loadAudios/{token}")]
        public ApiResponse<object> LoadMusic(Guid token)
        {
            ApiResponse<DataModels.User> response = _vkAuthService.CheckToken(token);
            if (!response.Ok)
                return new ApiResponse<object>(response.ErrorMessage);

            return _vkMusicService.LoadAudios(response.Data);
        }

        [HttpGet, Route("scheduleAudios")]
        public ApiResponse<IEnumerable<ApiModels.Audio>> GetAudios(Guid token, int scheduleId, int skip, int take)
        {
            ApiResponse<DataModels.User> response = _vkAuthService.CheckToken(token);
            if (!response.Ok)
                return new ApiResponse<IEnumerable<ApiModels.Audio>>(response.ErrorMessage);

            return _vkMusicService.GetAudios(response.Data, scheduleId, skip, take);
        }

        [HttpPost, Route("rawAudios")]
        public ApiResponse<IEnumerable<ApiModels.Audio>> GetAudios([FromUri] Guid token, [FromBody]AudioRequest request)
        {
            ApiResponse<DataModels.User> response = _vkAuthService.CheckToken(token);
            if (!response.Ok)
                return new ApiResponse<IEnumerable<ApiModels.Audio>>(response.ErrorMessage);

            return _vkMusicService.GetAudios(request);
        }

        [HttpGet, Route("newScheduleAudios")]
        public ApiResponse<IEnumerable<ApiModels.Audio>> GetAudiosNew(Guid token, int scheduleId, int skip, int take)
        {
            ApiResponse<DataModels.User> response = _vkAuthService.CheckToken(token);
            if (!response.Ok)
                return new ApiResponse<IEnumerable<ApiModels.Audio>>(response.ErrorMessage);

            return _vkMusicService.GetNewAudios(response.Data, scheduleId, skip, take);
        }
             
        [HttpGet, Route("myAudios")]
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
