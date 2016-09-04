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
        public readonly AuthService _vkService;

        public AuthController()
        {
            _vkService = new AuthService();
        }

        [HttpPost, Route("register")]
        public ApiResponse<DataModels.User> RegisterUser([FromBody]RegisterRequest request)
        {
            return _vkService.RegisterNewUser(request.Code);
        }

        [HttpPut, Route("updateToken")]
        public ApiResponse<DataModels.User> UpdateToken(Guid token)
        {
            return _vkService.UpdateToken(token);
        }

        [HttpGet, Route("test")]
        public string Test(string name)
        {
            return "Hello " + name;
        }

        protected override void Dispose(bool disposing)
        {
            _vkService.Dispose();
            base.Dispose(disposing);
        }
    }
}
