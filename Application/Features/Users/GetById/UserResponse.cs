﻿namespace Tickest.Application.Features.Users.GetById;

public sealed record UserResponse(
    Guid Id,
    string Name,
    string Email,
    List<string> Specialties
);


