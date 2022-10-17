using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Telegram.Domain.Entities;
using Telegram.Infrastructure;

namespace Web.AppBuilder
{
    internal static partial class AppBuilderExtensions
    {
        public static WebApplicationBuilder AddAuth(this WebApplicationBuilder builder)
        {
            builder.Services
            .AddControllers()
            .AddApplicationPart(Telegram.Presentation.AssemblyReference.Assembly);

            builder.Services.TryAddSingleton<ISystemClock, SystemClock>();
            builder.Services
            .AddIdentityCore<User>((options) =>
            {
                options.User.AllowedUserNameCharacters =
                "АаБбВвГгДдЕеЁёЖжЗзИиЙйКкЛлМмНнОоПпРрСсТтУуФфХхЦцЧчШшЩщЪъЫыЬьЭэЮюЯяabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager<SignInManager<User>>();



            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CORS", builder =>
                {
                    builder.SetIsOriginAllowed((url) =>
                    {
                        var uri = new Uri(url);
                        return uri.Host == "localhost" || uri.Host == "127.0.0.1";
                    })
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });


            return builder;
        }
    }
}
