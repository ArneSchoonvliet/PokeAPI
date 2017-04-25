using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Anime.Interfaces
{
    public interface IAnimeManager
    {
        Task<DAL_Database.DTO.Anime> GetById(int id);
        Task<List<DAL_Database.DTO.SearchAnime>> SearchByKeyword(string keyword, int pageSize);
    }
}