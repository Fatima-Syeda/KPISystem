using Microsoft.EntityFrameworkCore;
using Worker;
using Worker.Data;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql("Host=postgres;Port=5432;Database=eventsdb;Username=user;Password=password"));

builder.Services.AddHostedService<EventWorker>();

var host = builder.Build();

// 🔥 ADD THIS
using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

host.Run();