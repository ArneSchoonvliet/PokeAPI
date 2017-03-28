using System;
using BLL.Authentication;
using BLL.Authentication.Interfaces;
using BLL.Authentication.Options;
using DAL.DbContext;
using DAL.DbContext.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BLL.AutoMapper;
using BLL.Pokemon;
using BLL.Pokemon.Interfaces;
using BLL.Seed;
using BLL.Seed.Interfaces;
using DAL.AutoMapper;
using DAL.DbContext.Interfaces;
using DAL.DbContext.Repositories;
using DAL.Json;
using DAL.Json.Interfaces;
using PokeAPI.Helpers;
using PokeAPI.MiddleWare;

namespace PokeAPI
{
    public class Startup
    {
        private IConfigurationRoot Configuration { get; }

        // Key to use when signing the JWT keys
        // TODO Replace this with 'real' key in PROD
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("gxvxYHFqnXea5b5Ax98d"));

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            // Retrieve jwt option values from 'config' file
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            // Place values from 'config' file into a JwtIssueOptions object
            // This object will be available to inject using DI (IOptions)
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            //TODO Move this to DAL and place connection string in config file
            const string connection = @"Server=(localdb)\mssqllocaldb;Database=PokeApi;Trusted_Connection=True;";
            services.AddDbContext<PokeContext>(options => options.UseSqlServer(connection));

            // Add Identity membership system
            // This is the default 'login' / 'user' managment system in AspNet 
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<PokeContext>()
                .AddDefaultTokenProviders();

            // AutoMapper        
            services.AddMappings();

            // DI
            services.AddScoped<IAuthenticationManager, AuthenticationManager>();
            services.AddScoped<ISeedManager, SeedManager>();
            services.AddScoped<IPokemonManager, PokemonManager>();

            services.AddScoped<IPokeApiRepository, PokeApiRepository>();
            services.AddScoped<IPokemonRepository, PokemonRepository>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Add logging to output window in dev mode
            if (env.IsDevelopment())
            {
                loggerFactory.AddDebug();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseCors("CorsPolicy");

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });

            app.UseMvc();

            var seeder = app.ApplicationServices.GetService<ISeedManager>();
            seeder.Seed();
        }
    }
}
