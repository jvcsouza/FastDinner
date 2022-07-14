using FastDinner.Application.Common.Interfaces.Services;

namespace FastDinner.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}