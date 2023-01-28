using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using Telegram.Application.Behaviors;
using Telegram.Domain.Abstractions;
using Telegram.Domain.Entities;
using Telegram.Infrastructure;
using Telegram.Infrastructure.Abstractions;
using Telegram.Infrastructure.BackgroundJobs;
using Telegram.Infrastructure.Cash;
using Telegram.Infrastructure.Idempotence;
using Telegram.Infrastructure.Interceptors;
using Web.Middlewares;
using Web.Options;
using Scrutor;
using Telegram.Domain.ValueObjects;
using Newtonsoft.Json;

var l = JsonConvert.DeserializeObject<IDomainEvent>(
    @"{""$type"":""Telegram.Domain.DomainEvents.UserCreatedDomainEvent, Telegram.Domain"",""Id"":""079131c1-944f-4fdf-b092-93e37271f354"",""Login"":{""$type"":""Telegram.Domain.ValueObjects.Login, Telegram.Domain"",""Value"":""string123""}}",
    new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.All
    }
);

Console.WriteLine(l.Id);

return;

var builder = WebApplication.CreateBuilder(args);
// Add optioins through options pattern. All options defined here are injectable through IOptions<name>
builder.Services.ConfigureOptions<DatabaseOptionsSetup>();

// Set up configurations
builder.Services.AddMediatR(Telegram.Application.AssemblyReference.Assembly);

builder.Services.Scan(selector => selector.FromAssemblies(
    Telegram.Infrastructure.AssembyReference.Assembly)
    .AddClasses(false)
    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
    .AsImplementedInterfaces()
    .WithScopedLifetime());
// Add pipeline behavior for validation pipeline to work with validation results
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
// Add all validators for commands and queries
builder.Services.AddValidatorsFromAssembly(Telegram.Application.AssemblyReference.Assembly, includeInternalTypes: true);
// Set up Quartz (background worker)
builder.Services.AddQuartz(configure =>
{
    var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));
    configure.AddJob<ProcessOutboxMessagesJob>(jobKey)
    .AddTrigger(trigger =>
    {
        trigger.ForJob(jobKey)
        .WithSimpleSchedule(schedule =>
        {
            schedule.WithInterval(TimeSpan.FromSeconds(10))
                    .RepeatForever();
        });
    });
    configure.UseMicrosoftDependencyInjectionJobFactory();
});
builder.Services.AddQuartzHostedService();
// Set up Mediatr with Scrutor
builder.Services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));
builder.Services.Decorate<IUserRepository, UserRepositoryCache>();

// Add libraries services
builder.Services.AddMemoryCache();
builder.Services.AddSignalR();

// Add custom services
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();
builder.Services.AddSingleton<RaiseDomainEventsInterceptor>();

// Set up EF Core
builder.Services
            .AddEntityFrameworkNpgsql()
            .AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var dbOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>()!.Value;
                options.UseNpgsql(dbOptions.ConnectionString!, action =>
                {
                    action.EnableRetryOnFailure(dbOptions.MaxRetryCount);
                    action.CommandTimeout(dbOptions.CommandTimeout);
                });
                options.EnableDetailedErrors(dbOptions.EnableDetailedErrors);
                options.EnableSensitiveDataLogging(dbOptions.EnableSensetiveDataLogging);

                var interceptor = serviceProvider.GetRequiredService<RaiseDomainEventsInterceptor>();
                options.AddInterceptors(interceptor);
            });
// Set up controllers
builder.Services
    .AddControllers()
    .AddApplicationPart(Telegram.Presentation.AssemblyReference.Assembly); // All of controllers are in Presentation layer
// Set up CORS
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
// Set up Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt => opt.DisplayRequestDuration());
}
// Default middlewares
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("CORS");
app.MapControllers();

// Add custom middleware
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

// Data seed
using (var scope = app.Services.CreateScope())
{
    var unit = scope.ServiceProvider.GetRequiredService<IUnitOfWork<ApplicationDbContext>>();
    unit.DbContext.Database.EnsureDeleted();
    unit.DbContext.Database.EnsureCreated();
    var userRepo = scope.ServiceProvider.GetService<IUserRepository>()!;
    var chatRepo = scope.ServiceProvider.GetService<IChatRepository>()!;
    var blockRepo = scope.ServiceProvider.GetService<IBlockedUsersRepository>()!;
    var messageRepo = scope.ServiceProvider.GetService<IMessageRepository>()!;
    var folderRepo = scope.ServiceProvider.GetService<IFolderRepository>()!;
    var createFunc = delegate (string dsn, string login, string email, string pass, string avlink, string about)
    {
        return User.Create(
            DisplayName.Create(dsn).Value!,
            Login.Create(login).Value!,
            Email.Create(email).Value!,
            Password.Create(pass).Value!,
            avlink,
            About.Create(about).Value!
            );
    };
    var user1 = createFunc("Elizabet", "lll4", "el@gmail.com", "aA1234!22", "image.jpg", "");
    var user2 = createFunc("Elizabet1", "lll5", "el2@gmail.com", "aA1234!22", "image.jpg", "");
    var user3 = createFunc("Elizabet2", "lll6", "el3@gmail.com", "aA1234!22", "image.jpg", "");
    var chat = Chat.Create();
    await userRepo.CreateAsync(user1);
    await userRepo.CreateAsync(user2);
    await userRepo.CreateAsync(user3);

    chatRepo.Add(chat);

    blockRepo.Add(user1.BlockUser(user2.Id));
    blockRepo.Add(user2.BlockUser(user1.Id));
    blockRepo.Add(user2.BlockUser(user3.Id));

    var uc1 = user1.AddChat(chat.Id);
    var uc2 = user2.AddChat(chat.Id);

    userRepo.AppendChat(uc1);
    userRepo.AppendChat(uc2);

    folderRepo.Add(uc1.CreateFolder("Private"));
    folderRepo.Add(uc2.CreateFolder("Private"));

    messageRepo.Add(chat.SendMessage(user1.Id, user2.Id, "Hi!"));
    messageRepo.Add(chat.SendMessage(user2.Id, user1.Id, "Om, Hello?"));

    Console.WriteLine(unit.SaveChanges());
}

app.Run();
