using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using UsersApi.Converters;
using UsersApi.Factories;
using UsersApi.Repository;
using UsersApi.Services;
using UsersApi.Validators;

namespace UsersApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMemoryCache();
            services.AddDbContext<UsersDbContext>(
                options => options.UseSqlite(@"Data Source=users.db"));
            services.AddTransient<IUserValidator, UserValidator>();
            services.AddScoped<IUsersHandler, UsersHandler>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IUserFactory, UserFactory>();
            services.AddScoped<IUserConverter, UserConverter>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<ISubscriptionsHandler, SubscriptionsHandler>();
            services.AddScoped<ISubscriptionsRepository, SubscriptionsRepository>();
            services.AddScoped<ISubscriptionFactory, SubscriptionFactory>();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "UsersApi", Version = "v1"}); });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UsersApi v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}