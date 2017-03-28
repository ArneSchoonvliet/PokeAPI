using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace PokeAPI.Helpers
{
    public static class AutoMapperConfiguration
    {
        public static void AddMappings(this IServiceCollection services)
        {
            Mapper.Initialize(cfg =>
            { 
                // DAL (Projection)
                cfg.AddProfile(new DAL.AutoMapper.PokemonMappings());

                // BLL (Mapping)
                cfg.AddProfile(new BLL.AutoMapper.PokemonMappings());
            });

            services.AddSingleton(Mapper.Configuration);
            services.AddSingleton<IMapper>(sp => new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService));
        }
    }
}
