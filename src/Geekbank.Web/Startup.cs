using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Geekbank.Web.Data;
using Geekbank.Web.Models;
using Geekbank.Web.Services;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using SendGrid;
using AspNet.Security.OAuth.GitHub;

namespace Geekbank.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            var partialConfig = builder.Build();

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
            else
            {
                builder.AddAzureKeyVault(
                    $"https://{partialConfig["Azure:KeyVault:VaultName"]}.vault.azure.net/",
                    partialConfig["Azure:ActiveDirectory:ClientId"],
                    partialConfig["Azure:ActiveDirectory:ClientSecret"]
                );
            }
            
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            var dbOptions = new DbContextOptionsBuilder();
            if (Configuration["ASPNETCORE_ENVIRONMENT"] == "Production")
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(Configuration["Azure:SqlServer:ConnectionString"]);
                });
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(@"Server=(localdb)\\geekbank;Database=Geekbank;Trusted_Connection=True;");
                });
            }

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            services.AddRecaptcha(new RecaptchaOptions {
                SiteKey = Configuration["Recaptcha:SiteKey"],
                SecretKey = Configuration["Recaptcha:SecretKey"]
            });

            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddTransient(c => new SendGridClient(Configuration["SendGrid:ApiKey"]));

            // Add application services.
            services.AddTransient<IEmailSender, SendGridEmailSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment() || env.IsEnvironment("Testing"))
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

                // Browser Link is not compatible with Kestrel 1.1.0
                // For details on enabling Browser Link, see https://go.microsoft.com/fwlink/?linkid=840936
                // app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseTwitterAuthentication(new TwitterOptions()
            {
                ConsumerKey = Configuration["Twitter:ApiKey"],
                ConsumerSecret = Configuration["Twitter:ApiSecret"],
            });

            app.UseGoogleAuthentication(new GoogleOptions()
            {
                ClientId = Configuration["Google:ClientId"],
                ClientSecret = Configuration["Google:ClientSecret"],
            });

            app.UseGitHubAuthentication(new GitHubAuthenticationOptions()
            {
                ClientId = Configuration["GitHub:ClientId"],
                ClientSecret = Configuration["GitHub:ClientSecret"],
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
