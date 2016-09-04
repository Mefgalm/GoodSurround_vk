using GoodSurround.Logic.Response;
using GoodSurround.Logic.Vk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace GoodSurround.ApiControllers
{
    [RoutePrefix("api/v1/users")]
    public class UserController : ApiController
    {
        private readonly UserService _vkUserService;
        private readonly AuthService _vkAuthService;

        public UserController()
        {
            _vkUserService = new UserService();
        }

        [HttpGet, Route("searchUsers")]
        public ApiResponse<IEnumerable<ApiModels.User>> GetUsers(Guid token, string searchString, int skip, int take)
        {
            ApiResponse<DataModels.User> response = _vkAuthService.CheckToken(token);
            if (!response.Ok)
                return new ApiResponse<IEnumerable<ApiModels.User>>(response.ErrorMessage);

            return _vkUserService.GetUsers(searchString, skip, take);
        }

        [HttpGet, Route("user")]
        public ApiResponse<ApiModels.User> GetUser(Guid token)
        {
            ApiResponse<DataModels.User> response = _vkAuthService.CheckToken(token);

            //TODO: change this :)
            ApiResponse<ApiModels.User> apiResponse = new ApiResponse<ApiModels.User>()
            {
                Ok = response.Ok,
                ErrorMessage = response.ErrorMessage,
                Data = Logic.Mappers.ApiMapper.GetUser(response.Data),
            };

            return apiResponse;
        }

        protected override void Dispose(bool disposing)
        {
            _vkUserService.Dispose();
            _vkUserService.Dispose();
            base.Dispose(disposing);
        }
    }
}