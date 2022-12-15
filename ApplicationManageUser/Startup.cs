using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Cors.Infrastructure;
using BE.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using BE.DAL.Repository.UserRepository;
using ApplicationService.Catalog.Users;
using FluentValidation.AspNetCore;
using FluentValidation;
using ApplicationService.Model.UserModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace OpenProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<SystemDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddAutoMapper(typeof(Startup));
            //services.AddControllers();
            services.AddMvc();
            //services.AddTransient<IstorageService, FileStorageService>();
            services.AddTransient<IUserRepositories, UserRepositories>();
            services.AddTransient<IUserService, UserService>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.InitRedisCache(Configuration);
            //UnitOfWork
            services.AddTransient<IUnitOfWork<SystemDbContext>, UnitOfWork<SystemDbContext>>();
            //services.AddTransient<IValidator<CreateUserModel>, CreateUserValidator>();
            services.AddControllers()
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateUserValidator>());
            //swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger manager user", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,
                      },
                      new List<string>()
                    }
                });
            });
            string issuer = Configuration.GetValue<string>("Tokens:Issuer");
            string signingKey = Configuration.GetValue<string>("Tokens:Key");
            byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = issuer,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = System.TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
                };
            });
            //addcors
            services.AddCors(options => { options.AddPolicy(CorsSetting.DefaultPolicyName, InitCors(Configuration)); });
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors(CorsSetting.DefaultPolicyName);
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseRequestLocalization();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Manager user V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
        }
        public CorsPolicy InitCors(IConfiguration configuration)
        {
            var builder = new CorsPolicyBuilder();

            if (!string.IsNullOrEmpty(configuration[CorsSetting.AllowedOrigins]))
            {
                var origins = configuration[CorsSetting.AllowedOrigins].Split(";");
                builder.WithOrigins(origins);
            }
            else
            {
                builder.AllowAnyOrigin();
            }
            //Header
            if (!string.IsNullOrEmpty(configuration[CorsSetting.AllowedHeaders]))
            {
                var headers = configuration[CorsSetting.AllowedHeaders].Split(";");
                builder.WithHeaders(headers);
            }
            else
            {
                builder.AllowAnyHeader();
            }
            //Method
            if (!string.IsNullOrEmpty(configuration[CorsSetting.AllowedMethods]))
            {
                var methods = configuration[CorsSetting.AllowedMethods].Split(";");
                builder.WithMethods(methods);
            }
            else
            {
                builder.AllowAnyMethod();
            }
            //Credentials
            if (bool.TryParse(configuration[CorsSetting.AllowCredentials], out var _))
            {
                builder.AllowCredentials();
            }
            return builder.Build();
        }
        public class CorsSetting
        {
            /// <summary>
            /// 
            /// </summary>
            public static string DefaultPolicyName = "SiteCorsPolicy";

            /// <summary>
            /// 
            /// </summary>
            public static string AllowedOrigins = "Cors:AllowedOrigins";

            /// <summary>
            /// 
            /// </summary>
            public static string AllowedMethods = "Cors:AllowedMethods";

            /// <summary>
            /// 
            /// </summary>
            public static string AllowedHeaders = "Cors:AllowedHeaders";

            /// <summary>
            /// 
            /// </summary>
            public static string AllowCredentials = "Cors:AllowCredentials";
        }

    }
}

