using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using DAL_Database.Ef.Entities;
using DAL_Database.Ef.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL_Database.Ef.Repositories
{
    public class UserAnimeRepository : IUserAnimeRepository
    {
        private readonly EfContext _context;
        private readonly IMapper _mapper;
        private readonly DbSet<UserAnime> _dbSet;

        public UserAnimeRepository(EfContext ctx, IMapper mapper)
        {
            _context = ctx;
            _mapper = mapper;
            _dbSet = ctx.UserAnimes;
        }

        public async Task<bool> Any()
        {
            return await _dbSet.AnyAsync();
        }

        public async Task<bool> Any(Expression<Func<UserAnime, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public void Add(UserAnime entity)
        {
            _dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<UserAnime> entities)
        {
            _dbSet.AddRange(entities);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}