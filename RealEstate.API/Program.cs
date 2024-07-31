using RealEstate.Infrastructure.Dependency_Injection;
using RealEstate.Application.Services;
using RealEstate.API.ExceptionHandling;
using Stripe;
using Microsoft.AspNetCore.Mvc.Formatters;
using RealEstate.Infrastructure.SignalR;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;
using RealEstate.Infrastructure.Services.Subscription;
var builder = WebApplication.CreateBuilder(args);

// Load configuration based on the environment
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddExceptionHandler<AppExceptionHandler>();
builder.Services.InfrastructureServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {

        // builder.WithOrigins("https://wegapfrontend.azurewebsites.net")
        builder.AllowAnyOrigin()
             .AllowAnyHeader()
             .AllowAnyMethod()
             .WithExposedHeaders("*");
    });


});

var app = builder.Build();
// Configure Stripe
var configuration = builder.Configuration;
StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];

//app.UseCors(opt =>
//{
//    opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins(builder.Configuration["JWT:ClientUrl"]);
//});


app.UseSwagger();
app.UseSwaggerUI();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // This will show detailed error pages in development
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Handle exceptions in production
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseCors(o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("*"));
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