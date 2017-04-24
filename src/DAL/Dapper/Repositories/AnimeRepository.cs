using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DAL_Database.Dapper.Helpers;
using DAL_Database.Dapper.Interfaces;
using DAL_Database.DTO;
using DAL_Database.Ef.Entities;
using Microsoft.Extensions.Configuration;

namespace DAL_Database.Dapper.Repositories
{
    public class AnimeRepository : BaseRepository, IAnimeRepository
    {
        public AnimeRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<Anime> GetAnimeById(int id)
        {
            //var anime = 
            using (var dbConnection = Connection)
            {
                await dbConnection.OpenAsync();
                const string sql = "SELECT [Anime].[Id] ,[Anime].[Slug] ,[Anime].[AgeRating] ,[Anime].[EpisodeCount] ,[Anime].[EpisodeLength] ,[Anime].[Synopsis] ,[Anime].[YoutubeId] ,[Anime].[Created] ,[Anime].[LastUpdated] ,[Anime].[AverageRating] ,[Anime].[UserCount] ,[Anime].[AgeRatingDescription] ,[Anime].[ShowType] ,[Anime].[StartDate] ,[Anime].[EndDate] ,[Anime].[EnglishTitle] ,[Anime].[OriginalTitle] ,[Anime].[JapaneseTitle] ,[Genres].[Name] FROM Anime INNER JOIN AnimeGenres on Anime.Id = AnimeGenres.AnimeId INNER JOIN Genres on AnimeGenres.GenreId = Genres.Id WHERE [Anime].Id = @Id";
                return (await dbConnection.QueryParentChildAsync<Anime, string, int>(sql, a => a.Id, a => a.Genres, new {Id = id}, splitOn:"Name")).FirstOrDefault();
            }
        }

        public Task<IList<SearchAnime>> SearchForAnime(string keyword, int pageSize, int pageIndex = 0)
        {
            throw new NotImplementedException();
        }
    }
}
