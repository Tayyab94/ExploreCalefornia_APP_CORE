using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExploreCalefornia.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;

namespace ExploreCalefornia
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<FeatureToggle>(x=>new FeatureToggle
            {
                DeveloperException = configuration.GetValue<bool>("FeatureToggle:DeveloperException")
            });


            services.AddTransient<FormattingServices>();
            // Below line of code is use  to establish the database connection.....
            services.AddDbContext<ExploreCaleforiniaContext>(options =>
            {
                // This Connection string is define in AppSetting.Json File..
                var connectionString = configuration.GetConnectionString("BlogContext");

                options.UseSqlServer(connectionString);
            });
            services.AddDbContext<IdentityDataContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("IdentityDataContext");
                options.UseSqlServer(connectionString);
            });

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<IdentityDataContext>();
            services.AddMvc();

            //services.AddMvc(ac => ac.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,FeatureToggle featureToggle)
        {


            //By using this line first u have to disable the Developmetn environemt of your project. 
            //by clicking the property of your project. 
            app.UseExceptionHandler("/error.html");

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //if(configuration["FeatureToggling:DevelopmentException"] =="True")
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            if(featureToggle.DeveloperException)
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {

                if (context.Request.Path.Value.Contains("invalid"))
                    throw new Exception("Error!");

                await next();
            });

            app.UseAuthorization();

            //app.UseIdentity();
            app.UseRouting();

            app.UseEndpoints(endpoint =>
            {
                endpoint.MapControllerRoute("Default", "{controller=Home}/{action=Index}/{id?}");
            });



            ////  Services.AddMvc(x=>x.EnablingEndPont=false)  then this line of code work  Otherwise use the lines of Code as above
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute("Default", "{controller=Home}/{action=Index}/{Id?}"
            //        );
            //});

            //This is use to use the statics file like (HTMl, CSS)
            //app.UseFileServer();

            app.UseStaticFiles();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});
        }
    }
}
