namespace Tickest.Application.Features.Areas.Get;

public sealed record AreaResponse(
    Guid Id,  
    string Name,
    string Description,
    string Specialty
    );



