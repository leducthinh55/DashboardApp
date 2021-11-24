using DashboardApp.Domain.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DashboardApp.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Account> Accounts { get; set; }
        DbSet<Domain.Entities.Task> Tasks { get; set; }
        DbSet<Contact> Contacts { get; set; }
        DbSet<Dashboard> Dashboards { get; set; }
        DbSet<Widget> Widgets { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
