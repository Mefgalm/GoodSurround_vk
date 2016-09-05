using System.Web.Http;
using GoodSurround.Logic.Vk;
using GoodSurround.Logic.Response;
using System;
using GoodSurround.ApiModels.Messages;

namespace GoodSurround.ApiControllers
{
    [RoutePrefix("api/v1/auth")]
    public class AuthController : ApiController
    {
        public readonly AuthService _vkAuthService;

        public AuthController()
        {
            _vkAuthService = new AuthService();
        }

        [HttpPost, Route("register")]
        public ApiResponse<ApiModels.User> Register([FromBody]RegisterRequest request)
        {
            return _vkAuthService.RegisterNewUser(request.Code);
        }

        [HttpPut, Route("updateToken")]
        public ApiResponse<ApiModels.User> UpdateToken(Guid token)
        {
            return _vkAuthService.UpdateToken(token);
        }

        protected override void Dispose(bool disposing)
        {
            _vkAuthService.Dispose();
            base.Dispose(disposing);
        }
    }
}
