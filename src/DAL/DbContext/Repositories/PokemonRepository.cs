using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DAL.AutoMapper;
using DAL.DbContext.Entities;
using DAL.DbContext.Interfaces;
using DAL.DTO;
using Microsoft.EntityFrameworkCore;

namespace DAL.DbContext.Repositories
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly PokeContext _context;
        private readonly IMapper _mapper;
        private readonly DbSet<Pokemon> _dbSet;

        public PokemonRepository(PokeContext ctx, IMapper mapper)
        {
            _context = ctx;
            _mapper = mapper;
            _dbSet = ctx.Pokemons;
        }

        public async Task<bool> Any()
        {
            return await _dbSet.AnyAsync();
        }

        public async Task<bool> Any(Expression<Func<Pokemon, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<IList<PokemonListDto>> GetAllPokemons()
        {
            return await _dbSet.ProjectTo<PokemonListDto>().ToListAsync();
        }

        public void Add(Pokemon entity)
        {
            _dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<Pokemon> entities)
        {
            _dbSet.AddRange(entities);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}