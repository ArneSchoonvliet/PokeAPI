using AutoMapper;
using DAL_Database.AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
    public static class AutoMapperConfiguration
    {
        public static void AddMappings(this IServiceCollection services)
        {
            Mapper.Initialize(cfg =>
            { 
                // DAL (Projection)
                cfg.AddProfile(new AnimeMappings());

                // BLL (Mapping)
                cfg.AddProfile(new BLL.AutoMapper.AnimeMappings());
            });

            services.AddSingleton(Mapper.Configuration);
            services.AddSingleton<IMapper>(sp => new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService));
        }
    }
}
