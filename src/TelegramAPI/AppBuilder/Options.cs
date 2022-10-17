using Web.Options;

namespace Web.AppBuilder
{
    internal static partial class AppBuilderExtensions
    {
        public static WebApplicationBuilder ConfigureOptions(this WebApplicationBuilder builder)
        {
            builder.Services.ConfigureOptions<DatabaseOptionsSetup>();
            return builder;
        }
    }
}
