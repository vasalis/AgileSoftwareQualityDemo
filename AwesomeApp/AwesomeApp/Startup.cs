using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AwesomeApp
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
            services.AddControllers();
            services.AddSingleton<Container>(GetContainer);
            services.AddApplicationInsightsTelemetry();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private Container GetContainer(IServiceProvider options)
        {
            var lConnectionString = Configuration["CosmosDb:CosmosConnectionString"];
            var lCosmosDbName = Configuration["CosmosDb:CosmosDbName"]; 
            var lCosmosDbContainerName = Configuration["CosmosDb:CosmosDbContainerName"]; 
            var lCosmosDbPartionKey = Configuration["CosmosDb:CosmosDbPartitionKey"];

            var lClient = new CosmosClient(lConnectionString, new CosmosClientOptions
            {
                ConnectionMode = ConnectionMode.Direct
            });

            lClient.CreateDatabaseIfNotExistsAsync(lCosmosDbName).Wait();
            var lDb = lClient.GetDatabase(lCosmosDbName);
            lDb.CreateContainerIfNotExistsAsync(lCosmosDbContainerName, lCosmosDbPartionKey).Wait();

            return lDb.GetContainer(lCosmosDbContainerName);
        }
    }
}
