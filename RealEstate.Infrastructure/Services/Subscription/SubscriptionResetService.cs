using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RealEstate.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Services.Subscription
{
    public class SubscriptionResetService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SubscriptionResetService> _logger;

        public SubscriptionResetService(IServiceProvider serviceProvider, ILogger<SubscriptionResetService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                        var companies = await context.companies
          .Include(c => c.Subscription)
          .ToListAsync();

                        foreach (var company in companies)
                        {
                            if (company?.Subscription?.SubscriptionEndDate <= DateTime.UtcNow)
                            {
                                company.UsedPropertyCounts = 0; // Reset properties listed count
                            }
                        }

                        await context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while resetting properties listed count.");
                }

                await Task.Delay(TimeSpan.FromDays(1), stoppingToken); // Run daily
            }
        }
    }
}
