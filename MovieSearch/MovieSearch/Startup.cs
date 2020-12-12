using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using MovieSearch.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieSearch.Models;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using MovieSearch.Hubs;
using Microsoft.AspNetCore.Identity.UI.Services;
using MovieSearch.Services;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace MovieSearch
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
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString("DefaultConnection")));
            }
            else
            {

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString("AzureDb")
                        ));
            }


            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();


            services.AddControllersWithViews()
                .AddMvcLocalization(
                viewOptions => { viewOptions.ResourcesPath = "Resources"; },
                annotationsOptions =>
                {
                    annotationsOptions.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        var assemblyName = new AssemblyName(typeof(Resources.CommonResources).GetTypeInfo().Assembly.FullName);
                        return factory.Create(nameof(Resources.CommonResources), assemblyName.Name);
                    };
                });

            //services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("ru");
                options.SupportedCultures = new[] { new CultureInfo("en"), new CultureInfo("ru") };
                options.SupportedUICultures = new[] { new CultureInfo("en"), new CultureInfo("ru") };
            });


            services.AddRazorPages();
            services.AddSignalR();

            services.AddTransient<IEmailSender, EmailSender>(options =>
                new EmailSender(
                    Configuration["EmailSender:Host"],
                    Configuration.GetValue<int>("EmailSender:Port"),
                    Configuration["EmailSender:User"],
                    Configuration["EmailSender:Password"]
                )
            );

            services.Configure<ApplicationDbContext>(options => { options.Database.Migrate(); });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddProvider(new MovieSearch.Logger.FileLoggerProvider());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Shared/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseRequestLocalization();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "movies",
                    pattern: "{controller}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
                endpoints.MapHub<SignalRServer>("srServer");
            });

        }
    }
}
