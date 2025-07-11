using KERP.Application.Interfaces;
using KERP.Application.MassUpdate.PurchaseOrder.Commands;
using KERP.Application.MassUpdate.PurchaseOrder.Validation;
using KERP.Application.Shared.CQRS;
using KERP.BlazorUI.Components;
using KERP.Domain.Interfaces.MassUpdate.PurchaseOrder;
using KERP.Domain.Interfaces.Shared;
using KERP.Infrastructure.Persistence;
using KERP.Infrastructure.Persistence.Interceptors;
using KERP.Infrastructure.Persistence.Repositories;
using KERP.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddFluentUIComponents();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// A. Konfiguracja Identity (dodaj ten fragment)
builder.Services.AddDefaultIdentity<IdentityUser>()//options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<KerpDbContext>();

// B. Rejestracja Interceptora i DbContext
builder.Services.AddScoped<FactoryTenantInterceptor>();
builder.Services.AddDbContext<KerpDbContext>((sp, options) =>
    options.UseSqlServer(connectionString) // connectionString zdefiniowany wyżej
           .AddInterceptors(sp.GetRequiredService<FactoryTenantInterceptor>()));

// 2. Rejestracja dostępu do HttpContext (potrzebne dla CurrentUserService)
//builder.Services.AddHttpContextAccessor();

// 3. Rejestracja naszych interfejsów i ich implementacji
// Używamy AddScoped, co jest standardem dla operacji na żądanie w aplikacjach webowych.

// Infrastruktura
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
builder.Services.AddScoped<IPurchaseOrderReceiptDateUpdateRepository, PurchaseOrderReceiptDateUpdateRepository>();

// Aplikacja
if (builder.Environment.IsDevelopment())
{
    // W trybie deweloperskim, rejestrujemy naszą zaślepkę
    builder.Services.AddScoped<ICurrentUserService, FakeCurrentUserService>();
}
else
{
    // W każdym innym przypadku (Staging, Production), rejestrujemy prawdziwy serwis
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
}
builder.Services.AddScoped<IReceiptDateUpdateValidator, ReceiptDateUpdateValidator>();
builder.Services.AddScoped<ICommandHandler<PurchaseOrderReceiptDateUpdateCommand>, PurchaseOrderReceiptDateUpdateCommandHandler>();

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
