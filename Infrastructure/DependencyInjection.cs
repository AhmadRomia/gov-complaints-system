using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.Entities;
using Application.Common.Features.ComplsintUseCase;
using Application.Common.Behaviors;
using MediatR;
using Application.Common.Mappings;
using Application.Common.Models;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddScoped<AuditableEntityInterceptor>();


            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sql => sql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                ));

            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<ApplicationDbContext>()
            );

            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddScoped<JwtTokenService>();

            services.AddScoped<AuthenticationService>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));


            services.AddAutoMapper(cfg => { }, typeof(Application.AssemblyReference).Assembly);



            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

            services.AddTransient<IDateTimeService, DateTimeService>();

            services.AddScoped<IComplaintService, ComplaintService>();
            services.AddScoped<IOtpService, OtpService>();

            services.AddScoped<IEmailService, EmailService>();


            return services;
        }
    }
}
