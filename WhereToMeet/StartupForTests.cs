using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WhereToMeet.Database;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using WhereToMeet.Middleware.Authentication;
using System.IO;
using WhereToMeet.Services;
using WhereToMeet.Services.PlacesProviders;
using Microsoft.EntityFrameworkCore;

namespace WhereToMeet
{
    public class StartupForTests
    {
        public StartupForTests(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath + @"/../../..")
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddDbContext<ProgramDbContext>();
            services.AddSingleton<IDistanceResolver, GoogleDistanceMatrixResolver>();
            services.AddSingleton<IPlacesProvider, SimpleGooglePlacesProvider>();
            services.AddSingleton<IConfigurationRoot>(this.Configuration);
            //using (var dbContext = new ProgramDbContext())
            //{
            //    dbContext.Database.Migrate();
            //}
        }


        protected void ConfigureAuthentication(IApplicationBuilder app)
        {
            string saltIsReal = this.Configuration["SaltKey"]; // Encoding Key
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(saltIsReal));
            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = "ChungAngUniversity",

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = "Developers",

                // Validate the token expiry
                ValidateLifetime = false,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });

            var options = new TokenProviderOptions()
            {
                Audience = "Developers",
                Issuer = "ChungAngUniversity",
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            };
            app.UseMiddleware<TokenProviderMiddleware>(Options.Create(options));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            this.ConfigureAuthentication(app);
            app.UseMvc();
            app.UseStaticFiles();
            app.UseDeveloperExceptionPage();
        }
        
    }
}
