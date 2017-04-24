using System;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Anime.Interfaces;
using DAL_Database.Dapper.Interfaces;
using Microsoft.Extensions.Logging;

namespace BLL.Anime
{
    public class AnimeManager : IAnimeManager
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AnimeManager> _logger;
        private readonly IAnimeRepository _animeRepository;

        public AnimeManager(IAnimeRepository animeRepository, IMapper mapper, ILogger<AnimeManager> logger)
        {
            _mapper = mapper;
            _logger = logger;
            _animeRepository = animeRepository;
        }

        public async Task<DAL_Database.DTO.Anime> GetById(int id)
        {
            _logger.LogTrace($"Retrieving an Anime with id: {id}");

            var anime =  await _animeRepository.GetAnimeById(id);

            if(anime == null)
                throw new NullReferenceException($"Anime with id: {id} could not be found!");

            _logger.LogTrace($"Successfully retrieve {anime.EnglishTitle ?? anime.OriginalTitle}");

            return anime;
        }
    }
}
