using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL_Database.Ef.Entities;

namespace DAL_Database.Ef.Interfaces
{
    public interface IUserAnimeRepository
    {
        void Add(UserAnime entity);
        void AddRange(IEnumerable<UserAnime> entities);

        Task<bool> Any();
        Task<bool> Any(Expression<Func<UserAnime, bool>> predicate);

        Task Save();
    }
}