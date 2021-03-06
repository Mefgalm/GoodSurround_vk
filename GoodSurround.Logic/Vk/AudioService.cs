﻿using GoodSurround.Logic.Mappers;
using GoodSurround.Logic.Response;
using GoodSurround.VkModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodSurround.ApiModels;
using GoodSurround.ApiModels.Messages;

namespace GoodSurround.Logic.Vk
{
    public class AudioService : IDisposable
    {
        private readonly GoodSurroundDbContext _dbContext;
        private readonly WebService _vkWebService;
        private const int TokenExpiredAtDays = 2;

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public AudioService()
        {
            _dbContext = new GoodSurroundDbContext();
            _vkWebService = new WebService();
        }

        public ApiResponse<object> LoadAudios(DataModels.User user)
        {
            //get and remove all current user's albums/audios
            List<DataModels.Album> dataAlbumList = _dbContext.Albums.Where(x => x.UserId == user.Id).ToList();
            List<DataModels.Audio> dataAudioList = _dbContext.Audios.Where(x => x.UserId == user.Id).ToList();

            IEnumerable<int> audioIdList = dataAudioList.Select(x => x.VkId);

            List<DataModels.MixAudio> mixDataAudioList = _dbContext.MixAudios.Where(x => audioIdList.Contains(x.AudioId)).ToList();

            //Task loadAlbums = Task.Run(() =>
            //    dataAlbumList = _dbContext.Albums.Where(x => x.UserId == user.Id).ToList());

            //Task loadAudios = Task.Run(() =>
            //    dataAudioList = _dbContext.Audios.Where(x => x.UserId == user.Id).ToList());

            //await Task.WhenAll(loadAlbums, loadAudios);


            _dbContext.MixAudios.RemoveRange(mixDataAudioList);
            _dbContext.Audios.RemoveRange(dataAudioList);
            _dbContext.Albums.RemoveRange(dataAlbumList);

            //download all (okey, 6000)
            VkModels.AlbumEtity vkAlbumEntity = null;
            VkModels.AudioEtity vkAudioEntity = null;

            try
            {
                vkAlbumEntity = _vkWebService.GetAlbums(user.AccessToken, 100);
            } catch (Exception ex)
            {
                return new ApiResponse<object>(ex.Message);
            }

            try
            {       //TODO: mb add few Tasks for better perform? Need to check
                vkAudioEntity = _vkWebService.GetAudios(user.AccessToken, 6000);
            } catch (Exception ex)
            {
                return new ApiResponse<object>(ex.Message);
            }

            //album errors
            if (vkAlbumEntity.error != null)
                return new ApiResponse<object>(vkAlbumEntity.error?.error_msg);
            if (vkAlbumEntity.response == null)
                return new ApiResponse<object>("Album response is null");
            //audio errors
            if (vkAudioEntity.error != null)
                return new ApiResponse<object>(vkAudioEntity.error?.error_msg);
            if (vkAudioEntity.response == null)
                return new ApiResponse<object>("Audio response is null");

            user.AlbumsCount = vkAlbumEntity.response.items.Count();
            user.AudiosCount = vkAudioEntity.response.items.Count();
            _dbContext.Entry(user).State = System.Data.Entity.EntityState.Modified;

            //map albums
            IEnumerable<DataModels.Album> albums = vkAlbumEntity
                .response
                .items
                .Select(x =>
                {
                    DataModels.Album dataAlbum = VkMapper.GetAlbum(x);
                    dataAlbum.UserId = user.Id;
                    return dataAlbum;
                });            

            //map musics
            var audioList = new List<DataModels.Audio>(vkAudioEntity.response.items.Count());
            for(int i = 0; i < vkAudioEntity.response.items.Count(); i++)
            {
                var mappedAudio = VkMapper.GetAudio(vkAudioEntity.response.items.ElementAt(i));
                mappedAudio.Order = i + 1;
                mappedAudio.UserId = user.Id;

                audioList.Add(mappedAudio);
            }

            //add ranges and save changes
            _dbContext.Albums.AddRange(albums);
            _dbContext.SaveChanges();

            foreach(var aud in audioList)
            {
                DataModels.Album album = _dbContext.Albums.Local.FirstOrDefault(x => aud.AlbumId == x.VkId);
                if(album != null)
                {
                    aud.AlbumId = album.Id;
                }
            }

            _dbContext.Audios.AddRange(audioList);
            _dbContext.SaveChanges();

            return new ApiResponse<object>()
            {
                Ok = true
            };
        }

        public ApiResponse<IEnumerable<ApiModels.Audio>> GetNewAudios(DataModels.User user, int scheduleId, int skip, int take)
        {
            if (take <= 0)
                return new ApiResponse<IEnumerable<ApiModels.Audio>>()
                {
                    Ok = true,
                    Data = Enumerable.Empty<ApiModels.Audio>(),
                };

            skip = skip <= 0 ? 1 : skip + 1;

            IEnumerable<int> pageNumbers = Enumerable.Range(skip, take);

            var scheduleQry =
                (from s in _dbContext.Schedules
                 where s.Id == scheduleId
                 join sr in _dbContext.ScheduleRows on s.Id equals sr.ScheduleId
                 select new
                 {
                     Schedule = s,
                     ScheduleRow = sr
                 } into scheduleRows
                 group scheduleRows by scheduleRows.ScheduleRow.UserId into _s
                 select new
                 {
                     UserId = _s.Key,
                     ScheduleRows = _s,
                 } into userRows
                 from pn in pageNumbers
                 select userRows.ScheduleRows.Select(x => new
                 {
                     GroupIndex = x.ScheduleRow.GroupIndex
                                 + (userRows.ScheduleRows.Count() * (pn - 1)),
                     Order = x.ScheduleRow.Order
                                 + (x.Schedule.BlockSize * (pn - 1)),
                     UserId = x.ScheduleRow.UserId,

                 }))
                .SelectMany(x => x);

            List<ApiModels.Audio> audioList =
                (from s in scheduleQry

                 join a in _dbContext.Audios on
                               new { index = s.GroupIndex, userId = s.UserId }
                        equals new { index = a.Order, userId = a.UserId }
                 select new
                 {
                     Audio = a,
                     Order = s.Order,
                 })
                .OrderBy(x => x.Order)
                .ToList()
                .Select(x => ApiMapper.GetAudio(x.Audio))
                .ToList();

            for (int i = 0; i < audioList.Count(); i++)
            {
                AudioById audioEntity = _vkWebService.GetAudio(user.AccessToken, user.Id, audioList[i].VkId);

                string url = audioEntity?.response?.FirstOrDefault()?.url;
                audioList[i].Url = url;
            }

            return new ApiResponse<IEnumerable<ApiModels.Audio>>()
            {
                Ok = true,
                Data = audioList.Where(x => !string.IsNullOrWhiteSpace(x.Url)),
            };
        }

        public ApiResponse<IEnumerable<ApiModels.Audio>> GetAudios(AudioRequest request)
        {
            //builder user id list
            IEnumerable<int> userIds = request.Users.Select(x => x.UserId);

            //exclude old audios and get only audios for special users
            IQueryable<DataModels.Audio> audioQry = _dbContext
                .Audios
                .Where(x => !request.ExcludeAudios.Contains(x.Id)
                         && userIds.Contains(x.UserId))
                .OrderBy(x => Guid.NewGuid());

            var audioQryList = new List<IQueryable<DataModels.Audio>>();

            //builder queries
            foreach(var req in request.Users)
            {
                IQueryable<DataModels.Audio> userAudioQry = _dbContext
                    .Audios
                    .Where(x => x.UserId == req.UserId);

                if(!req.IsAudioMixes)
                {
                    userAudioQry = userAudioQry.OrderBy(x => x.Order);
                }

                userAudioQry = userAudioQry.Take(req.AudioCount);

                audioQryList.Add(userAudioQry);
            }

            IQueryable<DataModels.Audio> totalAudioQry = audioQryList.FirstOrDefault() ?? 
                Enumerable.Empty<DataModels.Audio>().AsQueryable();

            for(int i = 1; i < audioQryList.Count; i++)
            {
                totalAudioQry = totalAudioQry.Concat(audioQryList[i]);
            }

            //materialize audios
            return new ApiResponse<IEnumerable<ApiModels.Audio>>()
            {
                Ok = true,
                Data = totalAudioQry
                    .ToList()
                    .Select(x => ApiMapper.GetAudio(x)),                
            };
        }

        public ApiResponse<IEnumerable<ApiModels.Audio>> GetAudios(DataModels.User user, int skip, int take)
        {
            if (take <= 0)
                return new ApiResponse<IEnumerable<ApiModels.Audio>>()
                {
                    Ok = true,
                    Data = Enumerable.Empty<ApiModels.Audio>(),
                };

            if (skip < 0)
                skip = 0;


            List<ApiModels.Audio> audioList = _dbContext.Audios
                .Where(x => x.UserId == user.Id)
                .OrderByDescending(x => x.Date)
                .Skip(skip)
                .Take(take)
                .ToList()
                .Select(x => ApiMapper.GetAudio(x))
                .ToList();

            for (int i = 0; i < audioList.Count(); i++)
            {
                AudioById audioEntity = _vkWebService.GetAudio(user.AccessToken, user.Id, audioList[i].VkId);

                string url = audioEntity?.response?.FirstOrDefault()?.url;
                audioList[i].Url = url;
            }

            return new ApiResponse<IEnumerable<ApiModels.Audio>>()
            {
                Ok = true,
                Data = audioList.Where(x => !string.IsNullOrWhiteSpace(x.Url)),
            };
        }

        public ApiResponse<IEnumerable<ApiModels.Audio>> GetAudios(DataModels.User user, int scheduleId, int skip, int take)
        {
            if (take <= 0)
                return new ApiResponse<IEnumerable<ApiModels.Audio>>()
                {
                    Ok = true,
                    Data = Enumerable.Empty<ApiModels.Audio>(),
                };

            skip = skip <= 0 ? 1 : skip + 1;

            IEnumerable<int> pageNumbers = Enumerable.Range(skip, take);

            var scheduleQry =
                (from s in _dbContext.Schedules
                 where s.Id == scheduleId
                 join sr in _dbContext.ScheduleRows on s.Id equals sr.ScheduleId
                 select new
                 {
                     Schedule = s,
                     ScheduleRow = sr
                 } into scheduleRows
                 group scheduleRows by scheduleRows.ScheduleRow.UserId into _s
                 select new
                 {
                     UserId = _s.Key,
                     ScheduleRows = _s,
                 } into userRows
                 from pn in pageNumbers
                 select userRows.ScheduleRows.Select(x => new
                 {
                     GroupIndex = x.ScheduleRow.GroupIndex
                                 + (userRows.ScheduleRows.Count() * (pn - 1)),
                     Order = x.ScheduleRow.Order
                                 + (x.Schedule.BlockSize * (pn - 1)),
                     UserId = x.ScheduleRow.UserId,

                 }))
                .SelectMany(x => x);

            List<ApiModels.Audio> audioList =
                (from s in scheduleQry

                 join a in _dbContext.Audios on 
                               new { index = s.GroupIndex, userId = s.UserId } 
                        equals new { index = a.Order, userId = a.UserId }

                 join ma in _dbContext.MixAudios on
                               new { index = s.GroupIndex, userId = s.UserId }
                        equals new { index = ma.Order, userId = ma.UserId } into _ma

                 from ma in _ma.DefaultIfEmpty()

                 join m in _dbContext.Audios on  
                               new { audioId = ma.AudioId, userId = ma.UserId }
                        equals new { audioId = m.Id, userId = m.UserId } into _m
                 from m in _m.DefaultIfEmpty()

                 where ma == null || ma.ScheduleId == scheduleId
                 select new
                 {
                     Audio = m ?? a,
                     Order = s.Order,
                 })
                .OrderBy(x => x.Order)
                .ToList()
                .Select(x => ApiMapper.GetAudio(x.Audio))
                .ToList();

            for(int i = 0; i < audioList.Count(); i++)
            {
                AudioById audioEntity = _vkWebService.GetAudio(user.AccessToken, user.Id, audioList[i].VkId);

                string url = audioEntity?.response?.FirstOrDefault()?.url;
                audioList[i].Url = url;                
            }

            return new ApiResponse<IEnumerable<ApiModels.Audio>>()
            {
                Ok = true,
                Data = audioList.Where(x => !string.IsNullOrWhiteSpace(x.Url)),
            };
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
