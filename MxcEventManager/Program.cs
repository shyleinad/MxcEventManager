using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MxcEventManager.Data;

var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Load config from appsettings.json
var configuration = builder.Configuration;

// Add db contexts
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseSqlServer(
        configuration.GetConnectionString("DefaultIdentityConnection")
    ));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();

var app = builder.Build();

// Seed dbs
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.SeedAsync(services);
    await IdentitySeedData.IdentitySeedAsync(services);
}

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        x.SwaggerEndpoint("/swagger/v1/swagger.json", "MxcEventManager API");
        x.RoutePrefix = string.Empty;
    });
}

app.MapControllers();

await app.RunAsync();
