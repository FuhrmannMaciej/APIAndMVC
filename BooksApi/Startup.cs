using BooksApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace BooksApi
{
    public class Startup
    {
        private const string APIKEYNAME = "ApiKey";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddDbContext<BookContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("BookContext")));
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "BooksApi",
                    Version = "v1",
                    Description = "Books API, proste operacje CRUD",
                    Contact = new OpenApiContact { Name = "Maciej Fuhrmann", Email = "fuhrmann.maciej@gmail.com"},
                    License = new OpenApiLicense { Name = "Github", Url = new System.Uri("https://github.com/FuhrmannMaciej") }
                });
                c.AddSecurityDefinition(APIKEYNAME, new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Name = APIKEYNAME,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Prosze podaæ zakupiony klucz do API."
                });

                var key = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = APIKEYNAME
                    },
                    In = ParameterLocation.Header
                };

                var requirement = new OpenApiSecurityRequirement
                    { { key, new List<string>() }};

                c.AddSecurityRequirement(requirement);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BooksApi v1"));
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
