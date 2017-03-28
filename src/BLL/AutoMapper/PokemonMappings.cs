using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DAL.DbContext.Entities;
using DAL.DbContext.Enums;
using DAL.Json.Entities;

namespace BLL.AutoMapper
{
    public class PokemonMappings : Profile
    {
        public PokemonMappings()
        {
            CreateMap<PokemonJson, Pokemon>()
                .ForMember(e => e.PokedexId, opt=> opt.MapFrom(json => json.pkdx_id))
                .ForMember(e => e.ImageUrl, opt=> opt.MapFrom(json => $"https://img.pokemondb.net/artwork/{json.name}.jpg"))
                .ForMember(e => e.SpAttack, opt=> opt.MapFrom(json => json.sp_atk))
                .ForMember(e => e.SpDefense, opt=> opt.MapFrom(json => json.sp_def))
                .ForMember(e => e.Weight, opt=> opt.MapFrom(json => int.Parse(json.weight)/10.0))
                .ForMember(e => e.Height, opt=> opt.MapFrom(json => int.Parse(json.height)/10.0));
        }
    }
}
