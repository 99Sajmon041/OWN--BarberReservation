using BarberReservation.API.MiddleWare;
using BarberReservation.API.ServiceCollectionApiExtentions;
using BarberReservation.Application.Extensions;
using BarberReservation.Domain.Interfaces;
using BarberReservation.Infrastructure.Email;
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

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();

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
