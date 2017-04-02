using BLL.Authentication;
using BLL.Authentication.Interfaces;
using BLL.Pokemon;
using BLL.Pokemon.Interfaces;
using BLL.Seed;
using BLL.Seed.Interfaces;
using DAL.DbContext.Interfaces;
using DAL.DbContext.Repositories;
using DAL.Json;
using DAL.Json.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace PokeAPI.Helpers
{
    public static class DependencyRegistrationExtensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            // Register your business services here, e.g. services.AddTransient<IMyService, MyService>();
            services.AddScoped<IAuthenticationManager, AuthenticationManager>();
            services.AddScoped<ISeedManager, SeedManager>();
            services.AddScoped<IPokemonManager, PokemonManager>();
            return services;
        }

        public static IServiceCollection AddDataAccessServices(this IServiceCollection services)
        {
            // Register your business services here, e.g. services.AddTransient<IMyService, MyService>();
            services.AddScoped<IPokeApiRepository, PokeApiRepository>();
            services.AddScoped<IPokemonRepository, PokemonRepository>();
            return services;
        }
    }
}
