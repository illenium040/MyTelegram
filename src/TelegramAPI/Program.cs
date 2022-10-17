using Telegram.Domain.Abstractions;
using Telegram.Domain.Entities;
using Telegram.Infrastructure;
using Telegram.Infrastructure.Abstractions;
using Web.AppBuilder;

var builder = WebApplication.CreateBuilder(args);

//set up configurations
builder.Services.Scan(selector => selector.FromAssemblies(
    Telegram.Infrastructure.AssembyReference.Assembly
    )
    .AddClasses(false)
    .AsImplementedInterfaces()
    .WithScopedLifetime());

builder.Services.AddSignalR();

builder
    .ConfigureOptions()
    .AddAuth()
    .AddSwagger()
    .AddEFCore()
    .AddMediatrWithFluentValidation();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("CORS");

app.MapControllers();

//app.UseMiddleware<JwtMiddleware>();

//data seed
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

    var user1 = User.Create("Elizabet", "lll4", "el@gmail.com", "image.jpg", "");
    var user2 = User.Create("Elizabet1", "lll5", "el2@gmail.com", "image.jpg", "");
    var user3 = User.Create("Elizabet2", "lll6", "el3@gmail.com", "image.jpg", "");
    var chat = Chat.Create();
    await userRepo.CreateAsync(user1, "aA1234!");
    await userRepo.CreateAsync(user2, "aA1234!");
    await userRepo.CreateAsync(user3, "aA1234!");

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
