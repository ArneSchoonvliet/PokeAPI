using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DAL_Database.Dapper.Queries;
using DAL_Database.DTO;
using Microsoft.Extensions.Configuration;

namespace DAL_Database.Dapper.QueryHandlers
{
    public class SearchAnimeQueryHandler : BaseQueryHandler<SearchAnimeQuery, IList<SearchAnime>>
    {
        public SearchAnimeQueryHandler(IConfiguration configuration) : base(configuration)
        {
        }

        public override async Task<IList<SearchAnime>> Handle(SearchAnimeQuery message)
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

                var animeSearchList = await dbConnection.QueryAsync<SearchAnime>(sql, new { Keyword = message.Keyword, PageIndex = message.PageIndex, PageSize = message.PageSize });
                return animeSearchList.ToList();
            }
        }
    }
}
