using Florists.Application.Interfaces.Services;

namespace Florists.Infrastructure.Services
{
  public class DateTimeService : IDateTimeService
  {
    public DateTime UtcNow => DateTime.UtcNow;
  }
}
