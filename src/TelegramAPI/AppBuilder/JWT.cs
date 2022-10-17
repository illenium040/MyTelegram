using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Web.AppBuilder
{
    internal static partial class AppBuilderExtensions
    {
        public static WebApplicationBuilder AddJWT(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers(option =>
            {
                option.EnableEndpointRouting = false;
                var policy = new AuthorizationPolicyBuilder()
                                    .RequireAuthenticatedUser()
                                    .Build();
                option.Filters.Add(new AuthorizeFilter(policy));
            });
            //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //.AddJwtBearer(
            //        opt =>
            //        {
            //            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"]));
            //            opt.SaveToken = true;
            //            opt.RequireHttpsMetadata = false;
            //            opt.TokenValidationParameters = new TokenValidationParameters
            //            {
            //                ValidateIssuerSigningKey = true,
            //                IssuerSigningKey = key,
            //                ValidateAudience = false,
            //                ValidateIssuer = false,
            //                ValidateLifetime = true,
            //                ClockSkew = TimeSpan.Zero
            //            };
            //        });

            return builder;
        }
    }
}
