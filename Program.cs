using cush.Config;
using cush.Data;
using cush.Service;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApiDbContext>(opt =>
    opt.UseNpgsql(System.Environment.GetEnvironmentVariable("WebApiDatabase"))
        .UseSnakeCaseNamingConvention());      
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddScoped<CushService>();
builder.Services.AddSingleton<IDatabase> (
    redis => ConnectionMultiplexer.Connect(System.Environment.GetEnvironmentVariable("RedisConnection"))
        .GetDatabase());

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5296);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseDeveloperExceptionPage();
}

// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//     var context = services.GetRequiredService<ApiDbContext>();
//     context.Database.EnsureCreated();
//     context.Database.Migrate();

// }

app.UseHttpsRedirection();
app.MapControllers();
app.Run();