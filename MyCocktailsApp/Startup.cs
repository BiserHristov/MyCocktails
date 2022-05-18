namespace MyCocktailsApi
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using Microsoft.OpenApi.Models;
    using MyCocktailsApi.Data.Models;
    using MyCocktailsApi.Infrastructure;
    using MyCocktailsApi.Services;
    using MyCocktailsApi.Settings;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var mongoDbSettings = Configuration.GetSection(nameof(UserDatabaseSettings)).Get<UserDatabaseSettings>();

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>(mongoDbSettings.ConnectionString, mongoDbSettings.DatabaseName);

            services.Configure<CocktailDatabaseSettings>(Configuration.GetSection(nameof(CocktailDatabaseSettings)));
            services.Configure<UserDatabaseSettings>(Configuration.GetSection(nameof(UserDatabaseSettings)));

            services.AddSingleton<ICocktailDatabaseSettings>(provider =>
             provider.GetRequiredService<IOptions<CocktailDatabaseSettings>>().Value);

            services.AddSingleton<IUserDatabaseSettings>(provider =>
             provider.GetRequiredService<IOptions<UserDatabaseSettings>>().Value);

            services.AddAutoMapper(typeof(Startup));

            services.AddTransient<ICocktailService, CocktailService>();
            services.AddTransient<IUserService, UserService>();

            services.AddControllers().AddNewtonsoftJson(options => options.UseMemberCasing());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyCocktailsApp", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper)
        {
            Task
                .Run(async () =>
                {
                    await app.PrepareDatabase(mapper);
                })
                .GetAwaiter()
                .GetResult();

            app.UseDeveloperExceptionPage();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyCocktailsApp v1");
                    });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
