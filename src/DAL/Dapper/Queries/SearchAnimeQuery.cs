using System.Collections.Generic;
using DAL_Database.DTO;

namespace DAL_Database.Dapper.Queries
{
    public class SearchAnimeQuery : PaginatedQuery<IList<SearchAnime>>
    {
        public SearchAnimeQuery(string keyword, int pageSize, int pageIndex): base(pageSize, pageIndex)
        {
            Keyword = keyword;
        }

        public string Keyword { get; }
    }
}
