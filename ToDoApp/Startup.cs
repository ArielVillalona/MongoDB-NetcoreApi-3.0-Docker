using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ToDoApp.Config;
using ToDoApp.Models;
using ToDoApp.Models.Todo;

namespace ToDoApp
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
            //// requires using Microsoft.Extensions.Options
            //services.Configure<DatabaseSettings>(
            //    Configuration.GetSection(nameof(DatabaseSettings)));

            //services.AddSingleton<IDatabaseSettings>(sp =>
            //    sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);
            var config = new ServerConfig();
            Configuration.Bind(config);
            var todoContext = new TodoContext(config.MongoDB);
            var repo = new TodoRepository(todoContext);
            services.AddSingleton<ITodoRepository>(repo);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ToDo API",
                    Description = "Is a simple api test using docker, net core 3.0 and mongoDb",
                    Contact = new OpenApiContact
                    {
                        Name = "root",
                        Email = string.Empty
                    },
                    License = new OpenApiLicense
                    {
                        Name = "you can use"
                    }
                });
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Docker Api V1");
                c.RoutePrefix = string.Empty;
            });

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
