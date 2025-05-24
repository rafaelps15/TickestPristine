using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Features.Departments.Get;

public sealed record GetDepartmentsQuery(Guid id) : IQuery<List<DepartmentResponse>>;
