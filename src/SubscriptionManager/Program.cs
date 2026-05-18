using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SubscriptionManager.Data;
using SubscriptionManager.Interfaces;
using SubscriptionManager.Models.DTOs;
using SubscriptionManager.Repositories;
using SubscriptionManager.Services;
using SubscriptionManager.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=subscription.db"));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddValidatorsFromAssemblyContaining<CustomerDtoValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ensure database is created on first run
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("An unexpected error occurred.");
    });
});

// Customer endpoints
app.MapGet("/customers", async (ICustomerService svc) => await svc.GetAllCustomersAsync());
app.MapGet("/customers/{id:int}", async (int id, ICustomerService svc) =>
{
    var customer = await svc.GetCustomerByIdAsync(id);
    return customer is null ? Results.NotFound() : Results.Ok(customer);
});
app.MapPost("/customers", async (CustomerDto dto, ICustomerService svc, IValidator<CustomerDto> validator) =>
{
    var validationResult = await validator.ValidateAsync(dto);
    if (!validationResult.IsValid)
        return Results.ValidationProblem(validationResult.ToDictionary());
    return Results.Ok(await svc.CreateCustomerAsync(dto));
});
app.MapPut("/customers/{id:int}", async (int id, CustomerDto dto, ICustomerService svc, IValidator<CustomerDto> validator) =>
{
    var validationResult = await validator.ValidateAsync(dto);
    if (!validationResult.IsValid)
        return Results.ValidationProblem(validationResult.ToDictionary());
    return Results.Ok(await svc.UpdateCustomerAsync(id, dto));
});
app.MapDelete("/customers/{id:int}", async (int id, ICustomerService svc) => await svc.DeleteCustomerAsync(id));

// Subscription endpoints
app.MapGet("/subscriptions", async (ISubscriptionService svc) => await svc.GetAllSubscriptionsAsync());
app.MapGet("/subscriptions/{id:int}", async (int id, ISubscriptionService svc) =>
{
    var subscription = await svc.GetSubscriptionByIdAsync(id);
    return subscription is null ? Results.NotFound() : Results.Ok(subscription);
});
app.MapPost("/subscriptions", async (SubscriptionDto dto, ISubscriptionService svc, IValidator<SubscriptionDto> validator) =>
{
    var validationResult = await validator.ValidateAsync(dto);
    if (!validationResult.IsValid)
        return Results.ValidationProblem(validationResult.ToDictionary());
    return Results.Ok(await svc.CreateSubscriptionAsync(dto));
});
app.MapPut("/subscriptions/{id:int}", async (int id, SubscriptionDto dto, ISubscriptionService svc, IValidator<SubscriptionDto> validator) =>
{
    var validationResult = await validator.ValidateAsync(dto);
    if (!validationResult.IsValid)
        return Results.ValidationProblem(validationResult.ToDictionary());
    return Results.Ok(await svc.UpdateSubscriptionAsync(id, dto));
});
app.MapDelete("/subscriptions/{id:int}", async (int id, ISubscriptionService svc) => await svc.DeleteSubscriptionAsync(id));

app.Run();
