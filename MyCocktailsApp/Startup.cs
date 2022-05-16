namespace MyCocktailsApp
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using Microsoft.OpenApi.Models;
    using MyCocktailsApp.Data;
    using MyCocktailsApp.Infrastructure;
    using MyCocktailsApp.Services;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<PostsDatabaseSettings>()

            //services.AddSingleton<IMongoClient, MongoClient>(s =>
            //{
            //    var uri = s.GetRequiredService<IConfiguration>()["MongoConnectionString"];
            //    return new MongoClient(uri);
            //});
            //services.AddTransient<IPostDatabaseSettings, PostsDatabaseSettings>();
            //services.AddTransient<IPostService, PostService>();
            //services.AddTransient<IMongoClient, MongoClient>();

            services.Configure<CocktailDatabaseSettings>(Configuration.GetSection(nameof(CocktailDatabaseSettings)));

            services.AddSingleton<ICocktailDatabaseSettings>(provider =>
             provider.GetRequiredService<IOptions<CocktailDatabaseSettings>>().Value);

            services.AddAutoMapper(typeof(Startup));

            //services.AddScoped<CocktailService>();
            services.AddTransient<ICocktailService, CocktailService>();

            services.AddControllers().AddNewtonsoftJson(options => options.UseMemberCasing());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyCocktailsApp", Version = "v1" });
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.PrepareDatabase();

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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
