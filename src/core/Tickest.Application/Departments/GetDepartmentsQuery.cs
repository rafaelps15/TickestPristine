using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Departments;

public sealed record GetDepartmentsQuery : IQuery<List<DepartmentResponse>>;
