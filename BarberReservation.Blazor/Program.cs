using BarberReservation.Blazor.Auth;
using BarberReservation.Blazor.Common;
using BarberReservation.Blazor.Components;
using BarberReservation.Blazor.Services.Auth;
using BarberReservation.Blazor.Services.Implementaions;
using BarberReservation.Blazor.Services.Interfaces;
using BarberReservation.Blazor.UI.Message;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRazorPages();

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthState>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<ProtectedLocalStorage>();
builder.Services.AddScoped<IApiClient, ApiClient>();

builder.Services.AddScoped<IMeService, MeService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IService, Service>();
builder.Services.AddScoped<IHairdresserService, HairdresserService>();
builder.Services.AddScoped<ILookUpsService, LookUpsService>();
builder.Services.AddScoped<IWorkingHoursService, WorkingHoursService>();

builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!);
});

builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapRazorPages();
app.MapFallbackToFile("/");

app.Run();