using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RealEstate.Application.Contracts;
using RealEstate.Application.Contracts.property;
using RealEstate.Application.Helpers;
using RealEstate.Application.Services;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Data;
using RealEstate.Infrastructure.Repo;
using RealEstate.Infrastructure.Repo.property;
using RealEstate.Infrastructure.Services;
using RealEstate.Infrastructure.SignalR;
using System.Text;


namespace RealEstate.Infrastructure.Dependency_Injection
{
    public static class ServiceContainer
    {
        public static IServiceCollection InfrastructureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default")));
            services.AddIdentityCore<User>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedEmail = true;
            }).AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>().AddEntityFrameworkStores<AppDbContext>()
            .AddSignInManager<SignInManager<User>>()
            .AddUserManager<UserManager<User>>()
            .AddDefaultTokenProviders();  //able to create token for email confirmation

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication which should be atleast 512 bits")),
                        ValidIssuer = configuration["JWT:Issuer"],
                        ValidateIssuer = true,
                        ValidateAudience = false

                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && 
                            path.StartsWithSegments("/hubs"))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;


                        }
                    };
                });
            services.AddSingleton<PresenceTracker>();
            //Repos
            services.AddScoped<IUser, UserRepo>();
            services.AddScoped<IAdmin, AdminRepo>();
            services.AddScoped<ICompany, CompanyRepo>();

            services.AddScoped<IProperty, PropertyRepo>();
            services.AddScoped<IPropertyPhoto, PropertyPhotoRepo>();
            services.AddScoped<INotification, NotificationRepo>();
            services.AddScoped<ICompanyService, companyService>();
            services.AddScoped<IAgent, AgentRepo>();
            services.AddScoped<IMessage, MessageRepo>();



            services.AddSignalR(); // Add SignalR service

            services.AddHttpContextAccessor();
        
            //Services
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<FileService>();
            services.AddScoped<StripeService>();
            services.AddScoped<StripeWebHookHandler>();
            services.AddScoped<IContextSeedService, ContextSeedService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<GetUserHelper>();
            

            //to respond with an Array containing error messages when the model state is invalid
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ActionContext =>
                {
                    var errors = ActionContext.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToArray();
                    var toReturn = new
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(toReturn);
                };
            });
            services.AddCors();

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {{
                    new OpenApiSecurityScheme
                    {
                        Reference=new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id=JwtBearerDefaults.AuthenticationScheme
                        }
                    },new string[]{ }
                    }
                });
                        });
            return services;
        }
    }
}
