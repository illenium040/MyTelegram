using FluentValidation;
using MediatR;
using Telegram.Application.Behaviors;

namespace Web.AppBuilder
{
    internal static partial class AppBuilderExtensions
    {
        public static WebApplicationBuilder AddMediatrWithFluentValidation(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

            builder.Services.AddMediatR(Telegram.Application.AssemblyReference.Assembly);
            builder.Services.AddValidatorsFromAssembly(Telegram.Application.AssemblyReference.Assembly, includeInternalTypes: true);

            return builder;
        }
    }
}
