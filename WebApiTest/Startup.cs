using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTest.Common.Configuration;

namespace WebApiTest
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
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<AppConfiguration>(Configuration.GetSection("AppConfiguration"));

            //services.AddControllersWithViews(options =>
            //{
            //    options.CacheProfiles.Add("Global", new CacheProfile
            //    {
            //        Duration = 200,
            //        Location = ResponseCacheLocation.Any,
            //        NoStore = false
            //    });
            //    options.CacheProfiles.Add("UserAgent", new CacheProfile
            //    {
            //        VaryByHeader = "User-Agent",
            //        Duration = 15,
            //        Location = ResponseCacheLocation.Client,
            //        NoStore = false
            //    });
            //    options.CacheProfiles.Add("NoCashe", new CacheProfile
            //    {
            //        Duration = 120,
            //        Location = ResponseCacheLocation.None,
            //        NoStore = false
            //    });
            //});

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.Use(async (context, next) =>
            //{
            //    context.Response.GetTypedHeaders().CacheControl =
            //     new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
            //     {
            //         Public = true,
            //         MaxAge = TimeSpan.FromSeconds(200),
            //         MustRevalidate = true,
            //     };

            //    await next();
            //});

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
        }
    }
}
