using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Departments.Get;

public sealed record GetDepartmentsQuery(Guid id) : IQuery<List<DepartmentResponse>>;
