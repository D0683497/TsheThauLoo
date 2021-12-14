using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using TsheThauLoo.Data;
using TsheThauLoo.Entities.Identity;
using TsheThauLoo.Services;
using TsheThauLoo.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TsheThauLooDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.IncludeErrorDetails = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = JwtClaims.Subject,
            RoleClaimType = JwtClaims.Roles,
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer"), // 簽發者
            ValidateAudience = true,
            ValidAudience = builder.Configuration.GetValue<string>("JwtSettings:Audience"), // 接收者
            ValidateIssuerSigningKey = false,
            RequireExpirationTime = true, // 是否有過期時間
            ValidateLifetime = true, // 驗證時間
            ClockSkew = TimeSpan.Zero, // Token 在這個時間內仍有效
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:Key")))
        };

        // https://sandrino.dev/blog/aspnet-core-5-jwt-authorization
    });

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
    options.SignIn.RequireConfirmedEmail = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.User.AllowedUserNameCharacters = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    options.User.RequireUniqueEmail = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 10;
    options.Lockout.AllowedForNewUsers = true;
    options.ClaimsIdentity.UserIdClaimType = JwtClaims.Subject; // GetUserAsync 再用的
    options.ClaimsIdentity.UserNameClaimType = JwtClaims.Username;
    options.ClaimsIdentity.EmailClaimType = JwtClaims.Email;
    options.ClaimsIdentity.RoleClaimType = JwtClaims.Roles;
})
    .AddRoles<ApplicationRole>()
    .AddDefaultTokenProviders()
    .AddSignInManager<SignInManager<ApplicationUser>>()
    .AddEntityFrameworkStores<TsheThauLooDbContext>();

builder.Services.AddSingleton<ITokenService, TokenService>();

// TODO: �^�����Y

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = new HeaderApiVersionReader("api-version");
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
