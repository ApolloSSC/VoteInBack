using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Text;
using VoteIn.Auth;
using VoteIn.BL.Calculateurs;
using VoteIn.BL.Interfaces;
using VoteIn.BL.Interfaces.Calculateurs;
using VoteIn.BL.Interfaces.Mapper;
using VoteIn.BL.Interfaces.Services;
using VoteIn.BL.Mapper;
using VoteIn.BL.Repositories;
using VoteIn.BL.Services;
using VoteIn.DAL;
using VoteIn.Mail;
using VoteIn.Model.Models;

namespace VoteIn
{
    public class Startup
    {
        /// <summary>
        /// The services
        /// </summary>
        public ServiceProvider Services;
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfiguration Configuration { get; }

        #region Ctors.Dtors
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="env">The env.</param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                    .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<VoteInContext>(options =>
            {
                //if (!IsUnitTestRunning())
                //{
                options.UseSqlServer(Configuration.GetConnectionString("VoteInConnection"), b => b.MigrationsAssembly("VoteIn"));
                //}
                //else
                //{
                //    options.UseInMemoryDatabase("UnitTestVoteIn_" + Guid.NewGuid().ToString("D")); // Unique name
                //}
            });

            services.AddIdentity<User, IdentityRole>(options => { options.Password.RequireNonAlphanumeric = false; })
                .AddEntityFrameworkStores<VoteInContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwtBearerOptions =>
                {
                    jwtBearerOptions.SaveToken = true;
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["SecretKey"])),
                        ValidAudience = TokenAuthOption.Audience,
                        ValidIssuer = TokenAuthOption.Issuer,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(0)
                    };
                }
            );


            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services
                .AddSignalR();

            services
                .AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling =
                                               Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization()
            ;

            services.Configure<SendGridOptions>(Configuration.GetSection("SendGrid"));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IVotingProcessRepository), typeof(VotingProcessRepository));
            services.AddScoped(typeof(IVotingProcessService), typeof(ScrutinService));
            services.AddScoped(typeof(ICalculateurFactory), typeof(CalculatorFactory));
            services.AddScoped(typeof(IMapperService), typeof(MapperService));
            services.AddScoped(typeof(ISuffrageRepository), typeof(SuffrageRepository));
            services.AddScoped(typeof(IEnveloppeRepository), typeof(EnveloppeRepository));
            services.AddScoped(typeof(IEmailSenderService), typeof(EmailSenderService));
            services.AddScoped(typeof(IFileService), typeof(FileService));

            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "VoteInAPI", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme() { In = "header", Description = "Please insert JWT with Bearer into field", Name = "Authorization", Type = "apiKey" });
            });

            Services = services.BuildServiceProvider();
        }

        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseAuthentication();

            app.UseCors("CorsPolicy");

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "api";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "VoteInAPI V1");
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<VoteInHub>("/signalr");
            });

            app.UseMvc();
        }
        /// <summary>
        /// Determines whether [is unit test running].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is unit test running]; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool IsUnitTestRunning()
        {
            return false;
        }
        #endregion
    }
}
