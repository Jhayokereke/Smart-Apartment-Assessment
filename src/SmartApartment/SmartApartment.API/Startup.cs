using Elasticsearch.Net;
using Elasticsearch.Net.Aws;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Nest;
using SmartApartment.Application;
using SmartApartment.Application.Contracts;
using SmartApartment.Domain.Models;
using SmartApartment.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<IManagementRepository, ManagementRepository>();
            services.AddSingleton<IElasticClient>(s =>
            {
                var httpConnection = new AwsHttpConnection(Configuration["AWS:OpenSearch.Region"]);

                var pool = new SingleNodeConnectionPool(new Uri(Configuration["AWS:OpenSearch.URL"]));
                var config = new ConnectionSettings(pool, httpConnection).EnableDebugMode();

                return new ElasticClient(config);
            });


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartApartment.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IManagementRepository manRepo, IPropertyRepository propRepo)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartApartment.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            Seed.SeedData(manRepo, propRepo, Configuration);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
