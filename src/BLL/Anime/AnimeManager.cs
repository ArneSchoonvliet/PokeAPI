using AutoMapper;
using BLL.Anime.Interfaces;
using DAL_Database.Ef.Interfaces;

namespace BLL.Anime
{
    public class AnimeManager : IAnimeManager
    {
        private readonly IMapper _mapper;
        private readonly IUserAnimeRepository _userAnimeRepository;

        public AnimeManager(IUserAnimeRepository userAnimeRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userAnimeRepository = userAnimeRepository;
        }
    }
}
