using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pizza.Data;
using Pizza.Models;
using Pizza.Services;

namespace Pizza
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            
            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddSingleton<DataService, DataService>();

            AddDbContext(services);

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 4;
                    options.Password.RequireUppercase = false;
                    options.Lockout.MaxFailedAccessAttempts = 3;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(20);

                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Founders", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimTypes.Name, "mgfeller@hsr.ch", "sgehrig@hsr.ch", "mstolze@hsr.ch");
                });

                options.AddPolicy("ElevatedRights", policy => 
                    policy.RequireRole("Administrator", "PowerUser", "BackupAdministrator")
                );
            });

            services.AddOptions();
            //services.Configure<FacebookSettings>(Configuration);
            services.Configure<FacebookSettings>(Configuration.GetSection("FacebookSettings"));

            services.AddSession();
            services.AddMvc();
            services.AddAntiforgery(options => options.FormFieldName = "X-XSRF-TOKEN");

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        public virtual void AddDbContext(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            

            if (env.IsDevelopment())
            {
                app.ApplicationServices.GetRequiredService<ApplicationDbContext>().Database.Migrate();
                

                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();

                app.ApplicationServices.GetService<DataService>().EnsureData(Configuration["admin-pwd"]);

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

           

            app.UseStaticFiles();

            app.UseIdentity();


            var settings = app.ApplicationServices.GetService<IOptions<FacebookSettings>>().Value;
            if (settings.AppId != null)
            {
                app.UseFacebookAuthentication(new FacebookOptions()
                {
                    AppId = settings.AppId,
                    AppSecret = settings.AppSecret
                });
            }


            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            //            app.UseSession( x=> x. configure: s => s.IdleTimeout = TimeSpan.FromMinutes(30));
            app.UseSession(new SessionOptions() {IdleTimeout = TimeSpan.FromMinutes(30)});


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id:int?}");


                routes.MapRoute(
                    name: "shopname",
                    template: "shopname",
                    defaults: new {controller = "Home", action = "name"});
            });
        }
    }

    public class FacebookSettings
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
    }
}
