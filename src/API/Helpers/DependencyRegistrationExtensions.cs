using BLL.Anime;
using BLL.Anime.Interfaces;
using BLL.Authentication;
using BLL.Authentication.Interfaces;
using DAL_Database.Ef.Interfaces;
using DAL_Database.Ef.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
    public static class DependencyRegistrationExtensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            // Register your business services here
            services.AddScoped<IAuthenticationManager, AuthenticationManager>();
            services.AddScoped<IAnimeManager, AnimeManager>();
            return services;
        }

        public static IServiceCollection AddDataAccessServices(this IServiceCollection services)
        {
            // Register your data access services here
            services.AddScoped<IUserAnimeRepository, UserAnimeRepository>();
            return services;
        }
    }
}
