using INTEX.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML.OnnxRuntime;
using INTEX.Models;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace INTEX
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();
            //connects the ONNX file to the web app
            services.AddSingleton<InferenceSession>(
                new InferenceSession("./wwwroot/mummy-6.onnx")
);

            //SERVICE TO CONNECT WITH DATABASE CONTEXT FILE - ADDED BY JARED
            services.AddDbContext<postgresContext>(options =>
           {
               options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
           });

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseHsts();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // This is the CSP Header below. We had it blocking everything except the resources we were using, but it was being too finicky and blocking some things we needed so we had to comment it out for now

            //app.Use(async (context, next) =>
            //{
            //    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; style-src ‘sha256-LLMSint/uIRJDDcg+FpbE8hXHWu1XGCX4w6kOG+He5s=’ 'sha256-wCaebHVhiA7IFD0Vg2zVjzE0agFnPKYqzJ5wsPhB2/s=' ‘sha256-nJ4X8OouhFxf1XORLrnL8iOirugq9h4hz5HyWnfmD94=’; img-src 'self' https://media.cnn.com/api/v1/images/stellar/prod/181106093431-egypt-giza-pyramids-file.jpg?q=w_1347,h_1796,x_826,y_0,c_crop; script-src 'self';");
            //    await next();
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
