// -------------------- Program.cs --------------------
using Application;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add Application + Infrastructure
builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration);
builder.Services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "API", Version = "v1" });

    // JWT Authorization
        options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

// CORS ?????? ?? Swagger UI ?????? Authorization header
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSwagger", policy =>
        policy
            .WithOrigins("http://localhost:5063") // origin ????? ?? Swagger UI
            .AllowAnyHeader() // ???? Authorization
            .AllowAnyMethod()
    );
});

// JWT Configuration
builder.Services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
var jwt = configuration.GetSection("Jwt").Get<JwtSettings>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,
            ValidateAudience = true,
            ValidAudience = jwt.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
        };


        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = ctx =>
            {
                var header = ctx.Request.Headers["Authorization"].ToString();
                if (!string.IsNullOrWhiteSpace(header) && header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    var token = header.Substring("Bearer ".Length).Trim();
                    if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                        token = token.Substring("Bearer ".Length).Trim();
                    ctx.Token = token;
                }
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = ctx =>
            {
                var logger = ctx.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogError("Authentication Failed: {Message}", ctx.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = ctx =>
            {
                var logger = ctx.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("Token Validated. Claims: {Claims}",
                    string.Join(", ", ctx.Principal.Claims.Select(c => $"{c.Type}={c.Value}")));
                return Task.CompletedTask;
            },
            OnChallenge = ctx =>
            {
                var logger = ctx.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogWarning("OnChallenge: {Error} - {Description}", ctx.Error, ctx.ErrorDescription);
                return Task.CompletedTask;
            }
        };
    });

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = ctx =>
    {
        ctx.Response.StatusCode = 401;  // Unauthorized instead of redirect
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = ctx =>
    {
        ctx.Response.StatusCode = 403; // Forbidden instead of redirect
        return Task.CompletedTask;
    };
});


// Current user service + Error handler
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddTransient<ErrorHandlerMiddleware>();

// Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("AgencyPolicy", policy => policy.RequireRole("Agency"));
    options.AddPolicy("CitizenPolicy", policy => policy.RequireRole("Citizen"));
});

var app = builder.Build();

// Database seeding
using (var scope = app.Services.CreateScope())
{
    await DatabaseSeeder.SeedAsync(scope.ServiceProvider);
}
    app.UseSwagger();
    app.UseSwaggerUI();


app.UseStaticFiles();

// Logging Authorization header
app.Use(async (context, next) =>
{
    var auth = context.Request.Headers["Authorization"].ToString();
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Authorization Header: {Header}", auth);
    await next();
});

app.UseCors("AllowSwagger");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();
app.Run();
