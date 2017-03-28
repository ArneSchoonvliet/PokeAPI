using AutoMapper;
using BLL.Pokemon.ViewModels;
using DAL.DTO;
using DAL.Json.Entities;

namespace BLL.AutoMapper
{
    public class PokemonMappings : Profile
    {
        public PokemonMappings()
        {
            #region Seeding
            CreateMap<PokemonJson, DAL.DbContext.Entities.Pokemon>()
                .ForMember(e => e.PokedexId, opt=> opt.MapFrom(json => json.pkdx_id))
                .ForMember(e => e.ImageUrl, opt=> opt.MapFrom(json => $"https://img.pokemondb.net/artwork/{json.name.ToLower()}.jpg"))
                .ForMember(e => e.SpAttack, opt=> opt.MapFrom(json => json.sp_atk))
                .ForMember(e => e.SpDefense, opt=> opt.MapFrom(json => json.sp_def))
                .ForMember(e => e.Weight, opt=> opt.MapFrom(json => int.Parse(json.weight)/10.0))
                .ForMember(e => e.Height, opt=> opt.MapFrom(json => int.Parse(json.height)/10.0));
            #endregion

            #region Pokemon

            CreateMap<PokemonListDto, PokemonListViewModel>();

            #endregion

            #region UserPokemon

            // TODO

            #endregion
        }
    }
}
