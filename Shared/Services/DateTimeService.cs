using Application.Interfaces;

namespace Shared.Services
{
    public class DateTimeService : IDatetimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}
