using AutoMapper;
using Digital.Data.Data;
using Digital.Infrastructure.Interface;
using Digital.Infrastructure.Mapper;
using Digital.Infrastructure.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using System.Reflection;
using System.Text;

namespace Digital_BE.Api.Extensions
{
    public static class StartupExtensions
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "DSS",
                    Version = "v1.0.1",
                    Description = "Digital Signature System",
                });

                c.UseInlineDefinitionsForEnums();
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Add Bearer Token Here",
                    Name = "Authorization",
                    // Type = SecuritySchemeType.ApiKey,
                    Type = SecuritySchemeType.Http,
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
                        Name = "Bearer",
                        //Scheme = "oauth2",
                        //In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
            });

            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                        options.SerializerSettings.Converters.Add(new StringEnumConverter()));
            services.AddSwaggerGenNewtonsoftSupport();
        }

        //public static void AddJWTAuthentication(this IServiceCollection services, IConfiguration configuration)
        //{
        //    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        //    {
        //        options.TokenValidationParameters = new TokenValidationParameters
        //        {
        //            ValidateIssuer = true,
        //            ValidateLifetime = true,
        //            ValidateAudience = false,
        //            RequireAudience = false,
        //            ValidateIssuerSigningKey = true,
        //            ValidIssuer = configuration["JWT:Issuer"],
        //            IssuerSigningKey = new
        //                SymmetricSecurityKey
        //                (Encoding.UTF8.GetBytes
        //                    (configuration["JWT:Secret"]))
        //        };
        //    });
        //}

        public static void AddBusinessService(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IDocumentTypeService, DocumentTypeService>();
            services.AddScoped<IUserService, UserService>();
        }

        public static void ConfigCors(this IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("AllowAll", builder =>
                builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin())
            );
        }

        public static void AddAutoMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
             services.AddDbContext<ApplicationDBContext>(
                e =>
                {
                    e.EnableSensitiveDataLogging();
                    e.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                });
        }

        public static void ApplyPendingMigrations(this IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<ApplicationDBContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }

        //public static void AddHttpClients(this IServiceCollection services)
        //{
        //    services.AddHttpClient<ISystemManagementService, SystemManagementService>();
        //    services.AddHttpClient<IDocumentManagementService, DocumentManagementService>();
        //}
    }
}
