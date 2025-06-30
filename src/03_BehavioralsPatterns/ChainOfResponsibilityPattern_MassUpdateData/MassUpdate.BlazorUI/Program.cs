using MassUpdate.BlazorUI.Components;
using MassUpdate.Core.Factories;
using MassUpdate.Core.Interfaces;
using MassUpdate.Core.Interfaces.Repositories;
using MassUpdate.Infrastructure;
using MassUpdate.Infrastructure.Persistence.Repositories;
using MassUpdate.Infrastructure.Services;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddFluentUIComponents();

// Rejestrujemy serwisy potrzebne dla naszej fabryki
builder.Services.AddScoped<IOrderDataService, OrderDataService>();
builder.Services.AddScoped<IPurchaseOrderValidationRepository, OrderRepository>();

// Rejestrujemy naszą fabrykę, która będzie tworzyć walidatory
builder.Services.AddScoped<ValidatorFactory>();

// Rejestrujemy główny serwis, który używa fabryki
builder.Services.AddScoped<IMassUpdateValidationService, MassUpdateValidationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
