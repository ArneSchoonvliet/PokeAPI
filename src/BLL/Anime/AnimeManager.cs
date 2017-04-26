using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Anime.Interfaces;
using DAL_Database.Dapper.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BLL.Anime
{
    public class AnimeManager : IAnimeManager
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AnimeManager> _logger;
        private readonly IMediator _mediator;

        public AnimeManager(IMediator mediator, IMapper mapper, ILogger<AnimeManager> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DAL_Database.DTO.Anime> GetById(int id)
        {
            _logger.LogTrace($"Retrieving an Anime with id: {id}");
            var anime = await _mediator.Send(new AnimeQuery(id));

            if(anime == null)
                throw new NullReferenceException($"Anime with id: {id} could not be found!");

            _logger.LogTrace($"Successfully retrieve {anime.EnglishTitle ?? anime.OriginalTitle}");

            return anime;
        }

        public async Task<List<DAL_Database.DTO.SearchAnime>> SearchByKeyword(string keyword, int pageIndex)
        {
            keyword = keyword.ToLower();
            _logger.LogTrace($"Searching for an anime with keyword: {keyword}. Will use pageIndex: {pageIndex}!");

            var animeSearchList = await _mediator.Send(new SearchAnimeQuery(keyword: keyword, pageSize: 25, pageIndex: pageIndex));
            return animeSearchList.ToList();
        }
    }
}
