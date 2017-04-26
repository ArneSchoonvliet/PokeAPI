using BLL.Anime;
using BLL.Anime.Interfaces;
using BLL.Authentication;
using BLL.Authentication.Interfaces;
using DAL_Database.Dapper.QueryHandlers;
using DAL_Database.Ef.Interfaces;
using DAL_Database.Ef.Repositories;
using MediatR;
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
            // Mediator
            services.AddMediatR();

            // Dapper
            services.AddScoped<AnimeQueryHandler, AnimeQueryHandler>();
            services.AddScoped<SearchAnimeQueryHandler, SearchAnimeQueryHandler>();

            // Ef
            services.AddScoped<IUserAnimeRepository, UserAnimeRepository>();

            return services;
        }
    }
}
