using Telegram.Domain.Shared;

namespace Telegram.Infrastructure.Errors
{
    internal static partial class InfrastructureErrors
    {
        public static Error SaveChangesError(Exception e) =>
            new("UnitOfWork.Save", $"Message: {e.Message}\r\n InnerException: {e.InnerException?.Message}");
    }
}
