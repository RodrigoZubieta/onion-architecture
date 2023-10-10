

using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Threading;

namespace Persistance.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        private IDatetimeService _datetimeService;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDatetimeService datetimeService) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _datetimeService = datetimeService;
        }

        public DbSet<Cliente> clientes { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken()) 
        {
            foreach(var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = _datetimeService.NowUtc;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = _datetimeService.NowUtc;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
