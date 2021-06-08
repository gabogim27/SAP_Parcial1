using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SAP.Crypto.Domain.Implementation;
using SAP.Crypto.Repository.Implementations;
using SAP.Crypto.Repository.Interfaces;
using SAP.Crypto.Services.Interfaces;
using Services.Implementations;

namespace CryptoApp
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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SAP Crypto App",
                    Version = "v1",
                    Description = "Description for the API goes here.",
                    Contact = new OpenApiContact
                    {
                        Name = "Gabote Gimenez",
                        Email = string.Empty,
                    },
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy(
                    name: "AllowOrigin",
                    builder => {
                        builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });

            services.AddDbContext<DatabaseContext>(opt => opt.UseInMemoryDatabase("CriptoApp"));
            services.AddScoped<DbContext, DatabaseContext>();
            services.AddTransient(typeof(IRepository<Transaction>), typeof(Repository<Transaction>));
            services.AddTransient(typeof(IRepository<BankAccount>), typeof(Repository<BankAccount>));
            services.AddTransient(typeof(IRepository<CryptoAccount>), typeof(Repository<CryptoAccount>));
            services.AddTransient(typeof(IRepository<Customer>), typeof(Repository<Customer>));
            services.AddTransient(typeof(IRepository<Bank>), typeof(Repository<Bank>));
            services.AddTransient(typeof(IRepository<Currency>), typeof(Repository<Currency>));

            services.AddTransient(typeof(IAccountService), typeof(AccountService));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseCors("AllowOrigin");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SAP Crypto V1");

                // To serve SwaggerUI at application's root page, set the RoutePrefix property to an empty string.
                c.RoutePrefix = "swagger/ui";
            });
        }
    }
}
