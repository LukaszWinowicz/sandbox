using KERP.Application.Abstractions.Messaging;
using KERP.Application.Features.MassUpdate.Commands;
using KERP.Application.Interfaces.Services;
using KERP.Application.Interfaces.ValidationStrategies;
using KERP.Application.Services;
using KERP.Application.Validation;
using KERP.Application.ValidationStrategies;
using KERP.BlazorUI.Components;
using KERP.Domain.Identity;
using KERP.Domain.Interfaces.Repositories;
using KERP.Domain.Results;
using KERP.Infrastructure.Messaging;
using KERP.Infrastructure.Persistence;
using KERP.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddFluentUIComponents();

// 1. Rejestracja DbContext dla SQLServer.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AppDbContext>();

// 2. Rejestracja Repozytoriów
builder.Services.AddScoped<IPurchaseOrderValidationRepository, PurchaseOrderValidationRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); // Rejestracja generyczna
builder.Services.AddScoped<IUserRepository, UserRepository>();

// 3. Rejestracja Strategii Walidacji
builder.Services.AddScoped<IValidationStrategy<PurchaseOrderReceiptDateUpdateCommand>, PurchaseOrderReceiptDateUpdateStrategy>();

// 4. Rejestracja Serwisu Użytkownika
builder.Services.AddHttpContextAccessor(); // Potrzebne dla UserService
builder.Services.AddScoped<IUserService, UserService>();

// 5. Rejestracja Handlera i Mediatora
builder.Services.AddScoped<DiyMediator>();
builder.Services.AddTransient<IRequestHandler<PurchaseOrderReceiptDateUpdateCommand, List<RowValidationResult>>, PurchaseOrderReceiptDateUpdateCommandHandler>();

// 6. Rejestracja generycznego Pipeline Behavior
// To zarejestruje ValidationPipelineBehavior dla każdej kombinacji TRequest/TResponse
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

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
