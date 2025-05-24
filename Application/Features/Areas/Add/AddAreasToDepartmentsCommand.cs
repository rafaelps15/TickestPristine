using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Features.Areas.Add
{
    public record AddAreasToDepartmentsCommand(
        Guid DepartmentId,
        ICollection<Guid> AreaIds
    ) : ICommand<Guid>;
}
