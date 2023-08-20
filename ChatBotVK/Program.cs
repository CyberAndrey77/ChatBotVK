using ChatBotVK.Commands;
using ChatBotVK.Factories;
using ChatBotVK.Services;
using Database.Models;
using Database.Repositories;
using Microsoft.EntityFrameworkCore;
using VkNet;
using VkNet.Model;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connection));
builder.Services.AddScoped(
    options =>
    {
        var api = new VkApi();
        api.Authorize(new ApiAuthParams { AccessToken = builder.Configuration["AccessToken"] });
        return api;
    });
builder.Services.AddLogging();
builder.Services.AddScoped<IRepository<Category>,  CategoryRepository>();
builder.Services.AddScoped<IRepository<Thing>,  ThingRepository>();
builder.Services.AddSingleton<Commands>();
builder.Services.AddScoped<KeybordCreaterService>();
builder.Services.AddScoped <MessageCreaterService>();
builder.Services.AddScoped<ModelFactory>();
builder.Services.AddSingleton<SessionService>();
builder.Services.AddScoped<VkService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var commans = scope.ServiceProvider.GetRequiredService<Commands>();
    var categoryRepos = scope.ServiceProvider.GetRequiredService<IRepository<Category>>();
    commans.FillEquipmentCommands(categoryRepos);
}

app.Run("https://localhost:7234");
