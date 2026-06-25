using Tickest.SharedKernel;

namespace Tickest.Infrastructure.Time;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
