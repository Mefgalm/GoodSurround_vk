using GoodSurround.Logic.Mappers;
using GoodSurround.Logic.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodSurround.Logic.Vk
{
    public class UserService : IDisposable
    {
        private readonly GoodSurroundDbContext _dbContext;

        public UserService()
        {
            _dbContext = new GoodSurroundDbContext();
        }

        public ApiResponse<IEnumerable<ApiModels.User>> GetUsers(string searchString, int skip, int take)
        {
            if (take <= 0 || string.IsNullOrWhiteSpace(searchString))
                return new ApiResponse<IEnumerable<ApiModels.User>>()
                {
                    Ok = true,
                    Data = Enumerable.Empty<ApiModels.User>(),
                };

            if (skip < 0)
                skip = 0;

            int exludeMarker = int.MaxValue;

            IEnumerable<ApiModels.User> apiUserList =
                (from u in _dbContext.Users
                 select new
                 {
                     User = u,
                     Rank = u.LastName.StartsWith(searchString) ? 1 :
                            (u.FirstName + " " + u.LastName).StartsWith(searchString) ? 2 :
                            u.FirstName.StartsWith(searchString) ? 3 :
                            u.Id.ToString() == searchString ? 4 : exludeMarker,
                 } into _u
                 where _u.Rank != exludeMarker
                 orderby _u.Rank, _u.User.FirstName, _u.User.LastName
                 select _u.User)
                .Skip(skip)
                .Take(take)
                .ToList()
                .Select(x => ApiMapper.GetUser(x));

            return new ApiResponse<IEnumerable<ApiModels.User>>()
            {
                Data = apiUserList,
                Ok = true,
            };
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
