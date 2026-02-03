using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TreeWeb.Abstract;
using TreeWeb.AppContext;
using TreeWeb.Models;
using TreeWeb.Services;

namespace TreeWeb.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IHostApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tree API", Version = "v1" });
                // Настройка JWT в Swagger
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Введите токен JWT"
                };
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
            // Добавляем PasswordHasher
            builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
            // DbContext
            builder.Services.AddDbContext<TreeWebDbContext>(opt =>
                opt.UseSqlite("Data Source=tree.db"));

            // Аутентификация JWT
            var key = AuthOptions.GetSymmetricSecurityKey();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        
                        ValidateIssuer = false,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = false,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = key,
                        ValidateIssuerSigningKey = true
                    };
                });

            builder.Services.AddAuthorization();
            builder.Services.AddScoped<IDirectoryService, DirectoryService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
        }
    }
}
