using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NucuPaste.Api.Data;
using NucuPaste.Api.Domain.Repositories;
using NucuPaste.Api.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace NucuPaste.Api
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
            // Database Configuration
            services.AddDbContext<NucuPasteContext>(options =>
            {
                options.UseNpgsql(Configuration.GetSection("DatabaseConfig")["PostgresSQL"]);
            });

            // Application Services Configuration
            services.AddScoped<IEncrypt, EncryptService>();
            services.AddScoped<PasteRepository>();
            
            services.AddApiVersioning(options => { options.AssumeDefaultVersionWhenUnspecified = true; });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "NucuPaste API V1", Version = "v1" });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            // Configure Swagger and SwaggerUI
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "NucuPaste API");
                // Serve the SwaggerUI at the root.
                c.RoutePrefix = string.Empty;
            });

            app.UseMvc();
        }
    }
}
