using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tickest.Application.Abstractions.Data;
using Tickest.Domain.Entities.Auths;
using Tickest.Domain.Entities.Departments;
using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Entities.Specialties;
using Tickest.Domain.Entities.Tickets;
using Tickest.Domain.Entities.Users;
using Tickest.SharedKernel;

namespace Tickest.Persistence.Data;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IPublisher publisher)
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.UseEntityIdConversions();
        modelBuilder.HasDefaultSchema(Schemas.Default);
        modelBuilder.UseSnakeCaseNames<ApplicationDbContext>();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        await PublishDomainEventsAsync(cancellationToken);

        return result;
    }

    private async Task PublishDomainEventsAsync(CancellationToken cancellationToken)
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
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

internal static class EntityIdModelBuilderExtensions
{
    private static readonly ValueConverter<EntityId, Guid> EntityIdConverter = new(
        id => id.Value,
        value => new EntityId(value));

    public static void UseEntityIdConversions(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(EntityId))
                {
                    property.SetValueConverter(EntityIdConverter);
                }
            }
        }
    }
}
