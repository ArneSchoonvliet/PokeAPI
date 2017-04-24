using BLL.Anime;
using BLL.Anime.Interfaces;
using BLL.Authentication;
using BLL.Authentication.Interfaces;
using DAL_Database.Dapper.Interfaces;
using DAL_Database.Dapper.Repositories;
using DAL_Database.Ef.Interfaces;
using DAL_Database.Ef.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
    public static class DependencyRegistrationExtensions
    {
        // Register your business services here
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {

            services.AddScoped<IAuthenticationManager, AuthenticationManager>();
            services.AddScoped<IAnimeManager, AnimeManager>();
            return services;
        }

        // Register your data access services here
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services)
        {
            // Dapper
            services.AddScoped<IAnimeRepository, AnimeRepository>();

            // Ef
            services.AddScoped<IUserAnimeRepository, UserAnimeRepository>();

            return services;
        }
    }
}
