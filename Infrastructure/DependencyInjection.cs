using Application.Common.Interfaces;
using DashboardApp.Application.Common.Interfaces;
using DashboardApp.Infrastructure.Files;
using DashboardApp.Infrastructure.Persistence;
using Infrastructure.Encrytor;
using Infrastructure.JwtFactory;
using Infrastructure.Mails;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DashboardApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();
            services.AddTransient<IJwtTokenService, JwtTokenService>();
            services.AddTransient<IMailSender, MailSender>();
            services.AddTransient<IEncryptor, Encryptor>();

            //services.AddAuthentication();


            return services;
        }
    }
}