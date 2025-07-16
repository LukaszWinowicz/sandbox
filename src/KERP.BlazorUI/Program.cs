using KERP.Application.Abstractions.CQRS;
using KERP.Application.Abstractions.Dispatcher;
using KERP.Application.Abstractions.Repositories.MassUpdate.PurchaseOrder;
using KERP.Application.Common.Context;
using KERP.Application.Features.MassUpdate.PurchaseOrder.Commands.RequestUpdateReceiptDate;
using KERP.Application.Features.MassUpdate.PurchaseOrder.Queries.DTOs;
using KERP.Application.Features.MassUpdate.PurchaseOrder.Query.GetReceiptDateUpdates;
using KERP.BlazorUI.Components;
using KERP.Domain.Abstractions;
using KERP.Domain.Abstractions.Repositories.MassUpdate.PurchaseOrder;
using KERP.Domain.Abstractions.Results;
using KERP.Infrastructure.Auth;
using KERP.Infrastructure.Data;
using KERP.Infrastructure.Data.Repositories.MassUpdate.PurchaseOrder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddFluentUIComponents();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserContext, CurrentUserContext>();
builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();
builder.Services.AddScoped<IQueryDispatcher, QueryDispatcher>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Repositories
builder.Services.AddScoped<IReceiptDateUpdateRequestRepository, ReceiptDateUpdateRequestRepository>();
builder.Services.AddScoped<IPurchaseOrderReceiptDateUpdateReadRepository, PurchaseOrderReceiptDateUpdateReadRepository>();

// CQRS Handlers
builder.Services.AddScoped<ICommandHandler<RequestPurchaseOrderReceiptDateUpdateCommand, Result<bool>>, RequestPurchaseOrderReceiptDateUpdateCommandHandler>();
builder.Services.AddScoped<IQueryHandler<GetPurchaseOrderReceiptDateUpdateRequestsQuery, List<PurchaseOrderReceiptDateUpdateRequestDto>>, GetPurchaseOrderReceiptDateUpdateRequestsQueryHandler>();

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
