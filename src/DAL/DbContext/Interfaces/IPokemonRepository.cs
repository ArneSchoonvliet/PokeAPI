using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.DbContext.Entities;

namespace DAL.DbContext.Interfaces
{
    public interface IPokemonRepository
    {
        void Add(Pokemon entity);
        void AddRange(IEnumerable<Pokemon> entities);

        Task<bool> Any();
        Task<bool> Any(Expression<Func<Pokemon, bool>> predicate);

        Task<IList<Pokemon>> GetList();
        Task<IList<Pokemon>> GetList(Expression<Func<Pokemon, bool>> predicate);

        Task Save();
    }
}