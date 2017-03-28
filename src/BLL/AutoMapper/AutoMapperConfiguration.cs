using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        public static void AddMappings(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PokemonMappings());
            });

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
