using System.Collections.Generic;
using System.Threading.Tasks;
using DAL_Database.DTO;

namespace DAL_Database.Dapper.Interfaces
{
    public interface IAnimeRepository
    {
        Task<Anime> GetAnimeById(int id);
        Task<IList<SearchAnime>> SearchForAnime(string keyword, int pageSize, int pageIndex = 0);
    }
}
