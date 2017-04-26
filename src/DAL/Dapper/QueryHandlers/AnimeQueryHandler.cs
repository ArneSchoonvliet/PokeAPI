using System.Linq;
using System.Threading.Tasks;
using DAL_Database.Dapper.Helpers;
using DAL_Database.Dapper.Queries;
using DAL_Database.DTO;
using Microsoft.Extensions.Configuration;

namespace DAL_Database.Dapper.QueryHandlers
{
    public class AnimeQueryHandler : BaseQueryHandler<AnimeQuery, Anime>
    {
        public AnimeQueryHandler(IConfiguration configuration) : base(configuration)
        {
        }

        public override async Task<Anime> Handle(AnimeQuery message)
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

                var anime = await dbConnection.QueryParentChildAsync<Anime, string, int>(sql, a => a.Id, a => a.Genres, new { Id = message.Id }, splitOn: "Name");
                return anime.SingleOrDefault();
            }
        }
    }
}
