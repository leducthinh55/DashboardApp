using DashboardApp.Application.Common.Interfaces;
using DashboardApp.Domain.Common;
using DashboardApp.Domain.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace DashboardApp.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Domain.Entities.Task> Tasks { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Dashboard> Dashboards { get; set; }
        public DbSet<Widget> Widgets { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {                
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = _currentUserService.AccountId;
                    entry.Entity.Created = DateTime.Now;
                }
            }
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Entity<Account>().HasIndex(_ => _.Username).IsUnique();

            base.OnModelCreating(builder);
        }
    }
}
