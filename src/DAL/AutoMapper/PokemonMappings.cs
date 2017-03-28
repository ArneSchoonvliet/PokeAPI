using AutoMapper;
using DAL.DbContext.Entities;
using DAL.DTO;

namespace DAL.AutoMapper
{
    public class PokemonMappings : Profile
    {
        public PokemonMappings()
        {
            CreateMap<Pokemon, PokemonListDto>();

        }
    }
}
