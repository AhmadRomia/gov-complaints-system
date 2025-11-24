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

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {






            services.AddScoped<JwtTokenService>();

            services.AddScoped<AuthenticationService>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));


            services.AddScoped<RoleManager<IdentityRole<Guid>>>();
            services.AddScoped<UserManager<ApplicationUser>>();


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
