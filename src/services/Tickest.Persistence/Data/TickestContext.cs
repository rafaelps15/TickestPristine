using MediatR;
using Microsoft.EntityFrameworkCore;
using Tickest.Application.Abstractions.Data;
using Tickest.Domain.Entities.Auths;
using Tickest.Domain.Entities.Departments;
using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Entities.Specialties;
using Tickest.Domain.Entities.Tickets;
using Tickest.Domain.Entities.Users;
using Tickest.SharedKernel;

namespace Tickest.Persistence.Data;

public class TickestContext(DbContextOptions<TickestContext> options, IPublisher publisher)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Sector> Sectors { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Specialty> Specialties { get; set; }
    public DbSet<UserSpecialty> UserSpecialties { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Role> Roles { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        await PublishDomainEventsAsync(cancellationToken);

        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TickestContext).Assembly);
        modelBuilder.UseSnakeCaseNames<TickestContext>();
    }

    private async Task PublishDomainEventsAsync(CancellationToken cancellationToken)
    {
        var domainEvents = ChangeTracker
            .Entries()
            .Select(entry => entry.Entity)
            .OfType<Entity>()
            .SelectMany(entity =>
            {
                var domainEvents = entity.DomainEvents.ToArray();
                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToArray();

        foreach (var domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent, cancellationToken);
        }
    }
}
