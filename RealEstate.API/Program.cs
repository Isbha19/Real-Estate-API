using RealEstate.Infrastructure.Dependency_Injection;
using RealEstate.Application.Services;
using RealEstate.API.ExceptionHandling;
using Stripe;
using Microsoft.AspNetCore.Mvc.Formatters;
using RealEstate.Infrastructure.SignalR;
using Microsoft.Extensions.Options;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddExceptionHandler<AppExceptionHandler>();
builder.Services.InfrastructureServices(builder.Configuration);


var app = builder.Build();
// Configure Stripe
var configuration = builder.Configuration;
StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];

app.UseCors(opt =>
{
    opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins(builder.Configuration["JWT:ClientUrl"]);
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler(_ => { });
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");
app.MapHub<NotificationHub>("/hubs/notification");



app.MapControllers();

#region contextSeed
using var scope = app.Services.CreateScope();
try
{
    var contextSeedService = scope.ServiceProvider.GetService<IContextSeedService>(); // Resolve using the interface
    await contextSeedService.InitializeContextAsync();
}
catch (Exception ex)
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Failed to initialize and seed the database"); // Use the exception as the first parameter
}
#endregion


app.Run();