using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.DbContext.Entities;
using DAL.DTO;

namespace DAL.DbContext.Interfaces
{
    public interface IPokemonRepository
    {
        void Add(Pokemon entity);
        void AddRange(IEnumerable<Pokemon> entities);

        Task<bool> Any();
        Task<bool> Any(Expression<Func<Pokemon, bool>> predicate);

        Task<IList<PokemonListDto>> GetAllPokemons();

        Task Save();
    }
}