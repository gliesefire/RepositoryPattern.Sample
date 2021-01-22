using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Razor;
using RepositoryPattern.Abstractions;
using RepositoryPattern.Postgres;
using Microsoft.Extensions.Logging;
using RepositoryPattern.Sample.Persistence;
using RepositoryPattern.Sample;

namespace Bundling
{
    public class Startup
    {
        private IWebHostEnvironment CurrentEnvironment { get; set; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            if (env is null)
                throw new System.ArgumentException("Hosting Environment should not null", nameof(env));

            if (configuration is null)
                throw new System.ArgumentException("Configuration should not null", nameof(configuration));

            CurrentEnvironment = env;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //redundant addition 
            services.AddLogging();
            services.AddMvc(options =>
            {
                options.Conventions.Add(new FeatureConvention());
            })
            .AddRazorOptions(options => FeatureViewLocationExpander.SetRazorEngineConfig(options));

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddScoped<IConnectionFactory>(s =>
            {
                var loggerFactory = s.GetRequiredService<ILoggerFactory>();
                return new PostgresConnectionFactory(loggerFactory, connectionString);
            });
            services.AddScoped<DbContext>();
            services.AddScoped<IHomeRepository, HomeRepository>();
            services.AddScoped<IErrorRepository, ErrorRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
