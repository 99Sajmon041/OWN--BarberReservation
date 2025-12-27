using BarberReservation.API.MiddleWare;
using BarberReservation.API.ServiceCollectionApiExtentions;
using BarberReservation.Application.Extensions;
using BarberReservation.Infrastructure.Extensions;
using BarberReservation.Infrastructure.Seed;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, config) =>
{
    config
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services);    
});

builder.Services.AddTransient<ErrorHandlingMiddleware>();

builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddApi();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DefaultSeeder>();
    await seeder.SeedDefaultData();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
