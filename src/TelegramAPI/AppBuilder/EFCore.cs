using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Telegram.Infrastructure;
using Web.Options;

namespace Web.AppBuilder
{
    internal static partial class AppBuilderExtensions
    {
        public static WebApplicationBuilder AddEFCore(this WebApplicationBuilder builder)
        {
            builder.Services
            .AddEntityFrameworkNpgsql()
            .AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var dbOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>()!.Value;
                options.UseNpgsql(dbOptions.ConnectionString, action =>
                {
                    action.EnableRetryOnFailure(dbOptions.MaxRetryCount);
                    action.CommandTimeout(dbOptions.CommandTimeout);
                });
                options.EnableDetailedErrors(dbOptions.EnableDetailedErrors);
                options.EnableSensitiveDataLogging(dbOptions.EnableSensetiveDataLogging);

            });
            return builder;
        }
    }
}
