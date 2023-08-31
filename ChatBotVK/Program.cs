using ChatBotVK.Commands;
using ChatBotVK.Factories;
using ChatBotVK.Midellwares;
using ChatBotVK.Services;
using Database.Models;
using Database.Repositories;
using JorgeSerrano.Json;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
        {
            x.JsonSerializerOptions.PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy();
        });;
builder.Services.AddSwaggerGen();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connection));
builder.Services.AddHttpClient();
builder.Services.AddSingleton<SenderMessageService>();
builder.Services.AddSingleton<PhotoLoaderService>();
builder.Services.AddLogging();
builder.Services.AddScoped<IRepository<Category>, CategoryRepository>();
builder.Services.AddScoped<IRepository<Thing>, ThingRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepositoy>();
builder.Services.AddSingleton<Command>();
builder.Services.AddScoped<KeybordCreaterService>();
builder.Services.AddScoped<MessageCreaterService>();
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
app.UseMiddleware<Handler>();
app.Run("https://localhost:7234");
