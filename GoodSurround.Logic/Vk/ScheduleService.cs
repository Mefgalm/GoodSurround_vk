using GoodSurround.ApiModels.Messages;
using GoodSurround.Logic.Mappers;
using GoodSurround.Logic.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodSurround.Logic.Vk
{
    public class ScheduleService : IDisposable
    {
        private readonly GoodSurroundDbContext _dbContext;

        public ScheduleService()
        {
            _dbContext = new GoodSurroundDbContext();
        }

        public ApiResponse<object> CreateSchedule(DataModels.User dataUser, Schedule schedule)
        {
            if (schedule.Title == null)
                return new ApiResponse<object>("Schedule's title can't be null");

            schedule.Title = schedule.Title.Trim();

            if (schedule.Title == string.Empty)
                return new ApiResponse<object>("Schedule's title can't be empty");

            if (schedule.Title.Length > 50)
                return new ApiResponse<object>("Schedule's title can't be greater then 50 symbols");

            if (schedule.ScheduleRows == null || schedule.ScheduleRows.Count() == 0)
                return new ApiResponse<object>("Schedule rows can't be null or empty");            

            IEnumerable<int> orderList = schedule.ScheduleRows.Select(x => x.Order);
            List<int> sortOrderList = orderList.OrderBy(x => x).ToList();

            int sortOrderCount = sortOrderList.Last() - sortOrderList.First() + 1;

            if (sortOrderCount > sortOrderList.Count)            
                return new ApiResponse<object>("Order list has gaps");

            if (sortOrderCount < sortOrderList.Count)
                return new ApiResponse<object>("Order list has duplicates");

            IEnumerable<IGrouping<int, ScheduleRow>> scheduleRowsGroupIndex = schedule.ScheduleRows.GroupBy(x => x.UserId);

            //foreach (var sr in scheduleRowsGroupIndex)
            //{
            //    if (sr.GroupBy(x => x.CreateRandomOrder).Count() > 1)
            //        return new ApiResponse<object>($"User {sr.Key} have more then one order");
            //}

            DataModels.Schedule dataSchedule = new DataModels.Schedule()
            {
                BlockSize = sortOrderList.Count,
                DateCreated = DateTime.UtcNow,
                Title = schedule.Title,
                UserId = dataUser.Id,
            };

            _dbContext.Schedules.Add(dataSchedule);
            _dbContext.SaveChanges();

            int addedScheduleId = dataSchedule.Id;

            var dataScheduleRowList = new List<DataModels.ScheduleRow>();

            foreach(var sr in scheduleRowsGroupIndex)
            {
                ScheduleRow firstScheduleRow = sr.First();
                //if(firstScheduleRow.CreateRandomOrder)
                //{
                //    CreateRandomOrderForUser(firstScheduleRow.UserId, addedScheduleId);
                //}

                for(int i = 0; i < sr.Count(); i++)
                {
                    dataScheduleRowList.Add(new DataModels.ScheduleRow()
                    {
                        GroupIndex = i + 1,
                        Order = sr.ElementAt(i).Order,
                        ScheduleId = addedScheduleId,
                        UserId = firstScheduleRow.UserId,
                    });
                }
            }

            _dbContext.ScheduleRows.AddRange(dataScheduleRowList);
            _dbContext.SaveChanges();

            return new ApiResponse<object>()
            {
                Ok = true,
                Data = dataSchedule,
            };
        }

        public ApiResponse<object> RemoveSchedule(DataModels.User dataUser, int scheduleId)
        {
            var scheduleInfo =
                (from s in _dbContext.Schedules
                join sr in _dbContext.ScheduleRows on s.Id equals sr.ScheduleId into _sr
                join ma in _dbContext.MixAudios on s.Id equals ma.ScheduleId into _ma
                where s.Id == scheduleId
                select new
                {
                    Schedule = s,
                    ScheduleRows = _sr,
                    MixAudios = _ma,
                }).FirstOrDefault();

            if (scheduleInfo == null)
                return new ApiResponse<object>("Schedule not found");

            if (scheduleInfo.Schedule.UserId != dataUser.Id)
                return new ApiResponse<object>("You can remove only your schedule");

            _dbContext.MixAudios.RemoveRange(scheduleInfo.MixAudios);
            _dbContext.ScheduleRows.RemoveRange(scheduleInfo.ScheduleRows);
            _dbContext.Schedules.Remove(scheduleInfo.Schedule);

            _dbContext.SaveChanges();
            
            return new ApiResponse<object>()
            {
                Ok = true
            };
        }

        //private void CreateRandomOrderForUser(int userId, int scheduleId)
        //{
        //    List<DataModels.Audio> audioList = _dbContext
        //        .Audios
        //        .Where(x => x.UserId == userId)
        //        .ToList();

        //    ShuffleOrder(audioList);

        //    _dbContext.MixAudios.AddRange(audioList.Select(x => new DataModels.MixAudio()
        //    {
        //        Order = x.Order,
        //        AudioId = x.Id,
        //        ScheduleId = scheduleId,
        //        UserId = userId,
        //    }));
        //    _dbContext.SaveChanges();
        //}

        public void ShuffleOrder(List<DataModels.Audio> audioList)
        {
            int n = audioList.Count;
            Random rnd = new Random();
            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                int value = audioList[k].Order;
                audioList[k].Order = audioList[n].Order;
                audioList[n].Order = value;
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
