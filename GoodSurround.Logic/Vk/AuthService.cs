using GoodSurround.Logic.Mappers;
using GoodSurround.Logic.Response;
using GoodSurround.VkModels;
using NLog;
using System;
using System.Linq;

namespace GoodSurround.Logic.Vk
{
    //TODO: add interface
    public class AuthService : IDisposable
    {
        private readonly GoodSurroundDbContext _dbContext;
        private readonly WebService _vkWebService;
        private const int TokenExpiredAtDays = 2;

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public AuthService()
        {
            _dbContext = new GoodSurroundDbContext();
            _vkWebService = new WebService();
        }

        public ApiResponse<DataModels.User> CheckToken(Guid token)
        {
            DataModels.User dataUser = _dbContext.Users.FirstOrDefault(x => x.Token == token);

            if (dataUser == null)
                return new ApiResponse<DataModels.User>("User with this token not found");

            return new ApiResponse<DataModels.User>()
            {
                Ok = true,
                Data = dataUser,
            };            
        }

        public ApiResponse<ApiModels.AuthUser> UpdateToken(Guid token)
        {
            DataModels.User dataUser = _dbContext.Users.FirstOrDefault(x => x.Token == token);

            if(dataUser == null)            
                return new ApiResponse<ApiModels.AuthUser>("User with this token not found");
            
            dataUser.Token = Guid.NewGuid();
            dataUser.ExpiredAt = DateTime.UtcNow.AddDays(TokenExpiredAtDays);

            _dbContext.SaveChanges();

            return new ApiResponse<ApiModels.AuthUser>()
            {
                Ok = true,
                Data = ApiMapper.GetAuthUser(dataUser),
            };
        }

        public ApiResponse<ApiModels.AuthUser> RegisterNewUser(string code)
        {
            AccessToken vkAccessToken = _vkWebService.GetAccessToken(code);
            string accessToken = vkAccessToken.access_token;
            int userId = vkAccessToken.user_id;

            if(string.IsNullOrWhiteSpace(code))
            {
                return new ApiResponse<ApiModels.AuthUser>($"{nameof(code)} can't be null or white space");
            }
          
            UserEntity userEntity = null;

            try
            {
                userEntity = _vkWebService.GetUserEntity(accessToken, userId);
            } catch(Exception ex)
            {
                _logger.Error($"{nameof(RegisterNewUser)} failed: {ex.Message}");
                return new ApiResponse<ApiModels.AuthUser>(ex.Message);
            }

            if(userEntity == null)            
                return new ApiResponse<ApiModels.AuthUser>("Something went wrong");
            
            if(userEntity.error != null)            
                return new ApiResponse<ApiModels.AuthUser>(userEntity.error?.error_msg ?? "Unknown error");
            
            if(userEntity.response == null || userEntity.response.Count() != 1)            
                return new ApiResponse<ApiModels.AuthUser>("No user data");
            

            VkModels.User vkUser = userEntity.response.First();

            DataModels.User dataUser = _dbContext.Users.FirstOrDefault(x => x.Id == userId);
            if(dataUser == null)
            {
                dataUser = new DataModels.User()
                {
                    Id = userId,
                    FirstName = vkUser.first_name,
                    LastName = vkUser.last_name,
                    Photo50 = vkUser.photo_50,
                    AccessToken = accessToken,
                    Token = Guid.NewGuid(),
                    ExpiredAt = DateTime.UtcNow.AddDays(TokenExpiredAtDays),
                };

                _dbContext.Users.Add(dataUser);
            } else
            {
                dataUser.FirstName = vkUser.first_name;
                dataUser.LastName = vkUser.last_name;
                dataUser.Photo50 = vkUser.photo_50;
                dataUser.AccessToken = accessToken;
                dataUser.ExpiredAt = DateTime.UtcNow.AddDays(TokenExpiredAtDays);
            }

            _dbContext.SaveChanges();

            return new ApiResponse<ApiModels.AuthUser>()
            {
                Ok = true,
                Data = ApiMapper.GetAuthUser(dataUser),
            };
        }     

        public void Dispose()
        {
            _dbContext.Dispose();           
        }
    }
}
