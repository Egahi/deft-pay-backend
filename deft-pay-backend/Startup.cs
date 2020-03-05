using deft_pay_backend.DBContexts;
using deft_pay_backend.Models;
using deft_pay_backend.Repositories.Implementations;
using deft_pay_backend.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.Extensions.Logging;

namespace deft_pay_backend
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
            services.AddSingleton(Configuration);

            services.AddControllers();

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddCors();

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<MariaDbContext>()
                .AddDefaultTokenProviders();

            services.AddDbContext<MariaDbContext>(options =>
                options.UseMySql(Configuration["MariaDbConnectionString"].ToString(),
                    mySqlOptions =>
                    {
                        mySqlOptions.ServerVersion(new Version(10, 4, 8), ServerType.MariaDb);
                    }
            ), ServiceLifetime.Transient);

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Deft Pay Backend APIs", Version = "v1" });
                var filePath = Path.Combine(AppContext.BaseDirectory, "deft-pay-backend.xml");
                c.IncludeXmlComments(filePath);
            });

            services.AddScoped<IUserRepository, UserRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
                              IWebHostEnvironment env,
                              ILoggerFactory loggerFactory,
                              RoleManager<ApplicationRole> roleManager,
                              UserManager<ApplicationUser> userManager)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Deft Pay Backend APIs V1");
                c.RoutePrefix = string.Empty;
            });

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

            // Migrate Database
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<MariaDbContext>().Database.Migrate();
            }

            AppDbInitializer.SeedDatabase(Configuration,
                                          loggerFactory.CreateLogger<AppDbInitializer>(),
                                          roleManager,
                                          userManager);
        }
    }
}
