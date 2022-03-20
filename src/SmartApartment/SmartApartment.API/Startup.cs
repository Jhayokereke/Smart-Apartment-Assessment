using Elasticsearch.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Nest;
using SmartApartment.API.Middlewares;
using SmartApartment.Application;
using SmartApartment.Application.Contracts;
using SmartApartment.Infrastructure.Persistence;

namespace SmartApartment.API
{
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
            services.AddApplication();
            services.AddScoped<ISearchRepository, SearchRepository>();
            services.AddScoped<IBaseRepository, BaseRepository>();
            services.AddSingleton<IElasticClient>(s =>
            {
                //var httpConnection = new AwsHttpConnection(Configuration["AWS:OpenSearch.Region"]);

                //var pool = new SingleNodeConnectionPool(new Uri(Configuration["AWS:OpenSearch.URL"]));
                //var config = new ConnectionSettings(pool, httpConnection).EnableDebugMode();

                ////var config = new ConnectionSettings(new Uri("http://localhost:9200"));

                //return new ElasticClient(config);

                var settings = new ConnectionSettings(Configuration["Elastic:CloudID"], new BasicAuthenticationCredentials(Configuration["Elastic:Username"], Configuration["Elastic:Password"]))
                .EnableDebugMode();
                return new ElasticClient(settings);
            });


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartApartment.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBaseRepository baseRepo)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartApartment.API v1"));
            }

            //app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionHandler>();

            app.UseRouting();

            app.UseAuthorization();

            Seed.SeedData(baseRepo, Configuration);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
