using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DAL_Database.Dapper.Helpers;
using DAL_Database.Dapper.Interfaces;
using DAL_Database.DTO;
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
            using (var dbConnection = Connection)
            {
                await dbConnection.OpenAsync();
                const string sql = @"SELECT [Anime].[Id],
                                            [Anime].[Slug],
                                            [Anime].[AgeRating],
                                            [Anime].[EpisodeCount],
                                            [Anime].[EpisodeLength],
                                            [Anime].[Synopsis],
                                            [Anime].[YoutubeId],
                                            [Anime].[Created],
                                            [Anime].[LastUpdated],
                                            [Anime].[AverageRating],
                                            [Anime].[UserCount],
                                            [Anime].[AgeRatingDescription],
                                            [Anime].[ShowType],
                                            [Anime].[StartDate],
                                            [Anime].[EndDate],
                                            [Anime].[EnglishTitle],
                                            [Anime].[OriginalTitle],
                                            [Anime].[JapaneseTitle],
                                            [Genres].[Name] 
                                            FROM Anime 
                                            INNER JOIN AnimeGenres on Anime.Id = AnimeGenres.AnimeId 
                                            INNER JOIN Genres on AnimeGenres.GenreId = Genres.Id 
                                            WHERE [Anime].Id = @Id";

                var anime = await dbConnection.QueryParentChildAsync<Anime, string, int>(sql, a => a.Id, a => a.Genres, new {Id = id}, splitOn:"Name");
                return anime.SingleOrDefault();
            }
        }

        public async Task<IList<SearchAnime>> SearchForAnime(string keyword, int pageSize, int pageIndex)
        {
            using (var dbConnection = Connection)
            {
                await dbConnection.OpenAsync();
                const string sql = @"SELECT [Anime].[Id],
                                            [Anime].[AverageRating],
                                            [Anime].[ShowType],
                                            [Anime].[StartDate],
                                            [Anime].[EndDate],
                                            [Anime].[EnglishTitle],
                                            [Anime].[OriginalTitle]
                                            FROM Anime 
                                            WHERE CHARINDEX(@Keyword, OriginalTitle) > 0 OR CHARINDEX(@Keyword, EnglishTitle) > 0
                                            ORDER BY [Anime].[UserCount] DESC
                                            OFFSET @PageIndex ROWS
                                            FETCH NEXT @PageSize ROWS ONLY";

                var animeSearchList = await dbConnection.QueryAsync<SearchAnime>(sql, new {Keyword = keyword, PageIndex = pageIndex, PageSize = pageSize});
                return animeSearchList.ToList();
            }
        }
    }
}
