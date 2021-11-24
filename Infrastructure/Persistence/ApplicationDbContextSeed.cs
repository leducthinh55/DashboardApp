using DashboardApp.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace DashboardApp.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async System.Threading.Tasks.Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            // Seed, if necessary
            if (!context.Accounts.Any())
            {
                context.Accounts.Add(new Account
                {
                    FullName = "Le Duc Thinh",
                    Username = "thinhdle",
                    Email = "thinhdle@gmail.com",
                    Password = "123456"
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
