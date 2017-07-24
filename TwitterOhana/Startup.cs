using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StructureMap;
using StructureMap.Pipeline;
using TwitterOhana.Services;

namespace TwitterOhana
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("TwitterAuthenticated", policy => policy.RequireClaim("IsAuthenticated", "true"));
            });
            // Adds services required for using options.
            services.AddOptions();
            // Register the IConfiguration instance which MyOptions binds against.
            services.Configure<MyConfiguration>(Configuration.GetSection("MyCredentials"));

            // Add MVC services to the services container.
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ICredentialService, CredentialService>();
            return ConfigureIoC(services);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.EnvironmentName == "TwitterTest")
            {
                app.UseMiddleware<TwitterAuthenticationMiddleware>();
            }
            else if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationScheme = "CookieAuthentication",
                LoginPath = new PathString("/Home/Index"), 
                AccessDeniedPath = new PathString("/Home/Forbidden"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public IServiceProvider ConfigureIoC(IServiceCollection services)
        {
            var container = new Container();

            container.Configure(config =>
            {
                // Register stuff in container, using the StructureMap APIs...
                config.Scan(_ =>
                {
                    _.AssemblyContainingType(typeof(Startup));
                    _.WithDefaultConventions();
                });

                config.For(typeof(ITweetinviService), new SingletonLifecycle()).Add(typeof(TweetinviService));

                //Populate the container using the service collection
                config.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();

        }

        public class TwitterAuthenticationOptions : AuthenticationOptions
        {
            public ClaimsIdentity Identity { get; } = new ClaimsIdentity(new Claim[]
            {
                new Claim("ConsumerKey", "K0CvfPNmTrCc0djczVyxHz0xz"),
                new Claim("ConsumerSecret", "w3EG7V7g7GrtEm1LND1vNiKooc2zwTJRkM1XhPy2AmioRpX6kk"),
                new Claim("AccessToken", "696780677870706688-6W9L7oMQ9HUF2n0lQAOFfmbMuuNQKOg"),
                new Claim("AccessTokenSecret","4DHg9ZJ2eu6dRFiEmOijLrwBeLjliqfh60ej0vOlUuOhI")
            }, "TwitterAuthentication");
            public TwitterAuthenticationOptions()
            {
                this.AuthenticationScheme = "TestAuthenticationMiddleware";
                this.AutomaticAuthenticate = true;
                
            }
        }

        //logic to execute while handling an authentication
        public class TwitterAuthenticationHandler : AuthenticationHandler<TwitterAuthenticationOptions>
        {
            protected override Task<AuthenticateResult> HandleAuthenticateAsync()
            {
                var authenticationTicket = new AuthenticationTicket(
                    new ClaimsPrincipal(Options.Identity),
                    new AuthenticationProperties(),
                    this.Options.AuthenticationScheme);

                return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
            }
        }

        //define how to handle a request in a test context using our test authentication handler
        public class TwitterAuthenticationMiddleware : AuthenticationMiddleware<TwitterAuthenticationOptions>
        {
            private readonly RequestDelegate next;

            public TwitterAuthenticationMiddleware(RequestDelegate next, IOptions<TwitterAuthenticationOptions> options, ILoggerFactory loggerFactory, UrlEncoder UrlEncoder)
                : base(next, options, loggerFactory, UrlEncoder.Default)
            {
                this.next = next;
            }

            protected override AuthenticationHandler<TwitterAuthenticationOptions> CreateHandler()
            {
                return new TwitterAuthenticationHandler();
            }
        }

    }
}
