using GoodSurround.Logic.Vk;
using GoodSurround.Logic.Response;
using System.Web.Http;
using GoodSurround.ApiModels.Messages;
using System;

namespace GoodSurround.ApiControllers
{
    [RoutePrefix("api/v1/schedule")]
    public class ScheduleController : ApiController
    {
        public readonly AuthService _vkAuthService;
        public readonly ScheduleService _vkScheduleService;

        public ScheduleController()
        {
            _vkAuthService = new AuthService();
            _vkScheduleService = new ScheduleService();
        }

        [HttpPost, Route("create")]
        public ApiResponse<object> CreateSchedule(Guid token, [FromBody] Schedule schedule)
        {
            ApiResponse<DataModels.User> response = _vkAuthService.CheckToken(token);
            if (!response.Ok)
                return new ApiResponse<object>(response.ErrorMessage);

            return _vkScheduleService.CreateSchedule(response.Data, schedule);
        }

        [HttpDelete, Route("remove")]
        public ApiResponse<object> RemoveScheddule(Guid token, int scheduleId)
        {
            ApiResponse<DataModels.User> response = _vkAuthService.CheckToken(token);
            if (!response.Ok)
                return new ApiResponse<object>(response.ErrorMessage);

            return _vkScheduleService.RemoveSchedule(response.Data, scheduleId);
        }

        protected override void Dispose(bool disposing)
        {
            _vkAuthService.Dispose();
            _vkScheduleService.Dispose();
            base.Dispose(disposing);
        }
    }
}
