﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Data;
using Stripe;
using RealEstate.Application.Contracts;
using System.Numerics;
using RealEstate.Application.Extensions;
using RealEstate.Domain.Entities.CompanyEntity;
using RealEstate.Domain.Entities.SubscriptionEntity;


namespace RealEstate.Infrastructure.Services.Subscription
{
    public class StripeWebHookHandler
    {
        private readonly ILogger<StripeWebHookHandler> _logger;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        private readonly ICompany _company;
        private readonly NotificationService notificationService;

        public StripeWebHookHandler(ILogger<StripeWebHookHandler> logger, UserManager<User> userManager,
            AppDbContext context, ICompany company, NotificationService notificationService)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _company = company;
            this.notificationService = notificationService;
        }

        public async Task HandleEventAsync(Event stripeEvent)
        {
            try
            {
                switch (stripeEvent.Type)
                {
                    case Events.CheckoutSessionCompleted:
                        await HandleCheckoutSessionCompletedEvent(stripeEvent);
                        break;
                    case Events.CustomerSubscriptionCreated:
                        await HandleCustomerSubscriptionCreatedEvent(stripeEvent);
                        break;
                    case Events.CustomerSubscriptionDeleted:
                        await HandleCustomerSubscriptionDeletedEvent(stripeEvent);
                        break;
                    case Events.CustomerSubscriptionUpdated:
                        await HandleCustomerSubscriptionUpdatedEvent(stripeEvent);
                        break;
                    default:
                        _logger.LogWarning($"Unhandled event type: {stripeEvent.Type}");
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error handling Stripe event '{stripeEvent.Type}': {ex.Message}");
            }
        }

        private async Task HandleCheckoutSessionCompletedEvent(Event stripeEvent)
        {
            var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
            var customerId = session.CustomerId;

            var customerService = new CustomerService();
            var customer = await customerService.GetAsync(customerId);

            if (customer.Email != null)
            {
                var user = await _userManager.FindByEmailAsync(customer.Email);
                if (user != null)
                {
                    var response = await _company.ValidateUserForPayment();
                    bool isValidUser = response.Success;
                    if (!isValidUser)
                    {
                        _logger.LogWarning("User is not validated for payment.");
                        throw new Exception("User is not validated for payment.");
                    }

                }
                else
                {
                    _logger.LogError("No user found");
                    throw new Exception("No user found");
                }
            }
        }

        private async Task HandleCustomerSubscriptionCreatedEvent(Event stripeEvent)
        {
            var subscription = stripeEvent.Data.Object as Stripe.Subscription;
            if (subscription == null)
            {
                _logger.LogError("Stripe subscription data is null or invalid");
                throw new Exception("Stripe subscription data is null or invalid");
            }
            var subscriptionItem = subscription.Items.Data.FirstOrDefault();
            if (subscriptionItem == null)
            {
                _logger.LogError("No subscription item found in the Stripe subscription.");
                throw new Exception("No subscription item found in the Stripe subscription.");
            }

            var plan = subscriptionItem.Plan;


            var customerId = subscription.CustomerId;
            var customerService = new CustomerService();
            var customer = await customerService.GetAsync(customerId);

            if (customer.Email != null)
            {
                var user = await _userManager.FindByEmailAsync(customer.Email);
                if (user != null)
                {
                    // Validate user for payment

                    var company = await _context.companies
                        .Include(c => c.Subscription)
                        .FirstOrDefaultAsync(c => c.RepresentativeId == user.Id);
                    if (company != null)
                    {
                        var subscriptionService = new SubscriptionService();
                        var stripeSubscription = await subscriptionService.GetAsync(subscription.Id);
                        var invoiceService = new InvoiceService();
                        var latestInvoice = await invoiceService.ListAsync(new InvoiceListOptions
                        {
                            Subscription = subscription.Id,
                            Limit = 1
                        });

                        var amountPaid = latestInvoice.Data.FirstOrDefault()?.AmountPaid / 100m ?? 0m;


                        var newsubscription = new Domain.Entities.SubscriptionEntity.Subscription
                        {
                            StripeCustomerId = customerId,
                            StripeSubscriptionId = subscription.Id,
                            SubscriptionStatus = stripeSubscription.Status,
                            SubscriptionStartDate = DateTime.UtcNow,
                            SubscriptionEndDate = stripeSubscription.CurrentPeriodEnd,
                            IsActive = true,
                            CompanyId = company.Id,
                            PlanId = plan.Id,
                        };


                        company.Subscription = newsubscription;
                        company.SubscriptionAmtPaid += amountPaid; // Update amount paid

                        await _context.SaveChangesAsync();
                        if (!await _userManager.IsInRoleAsync(user, Constant.CompanyAdmin))
                        {
                            await _userManager.AddToRoleAsync(user, Constant.CompanyAdmin);
                        }
                        var planName = await _context.Plan
                .Where(p => p.Id == plan.Id)
                .Select(p => p.Name)
                .FirstOrDefaultAsync();

                        var userId = company.RepresentativeId;
                        var message = $"Payment Successful! Your company subscription to '{planName}' plan is now active. Your agents can now list properties and access advanced features.";
                        var url = "/company-dashboard";

                        await notificationService.NotifyUserAsync(userId, message, url);
                    }
                    else
                    {
                        _logger.LogWarning("Company not found for user.");
                    }
                }
                else
                {
                    _logger.LogError("No user found");
                    throw new Exception("No user found");
                }
            }
        }

        private async Task HandleCustomerSubscriptionDeletedEvent(Event stripeEvent)
        {
            var subscription = stripeEvent.Data.Object as Stripe.Subscription;
            var customerService = new CustomerService();
            var customer = await customerService.GetAsync(subscription.CustomerId);

            if (customer.Email != null)
            {
                var user = await _userManager.FindByEmailAsync(customer.Email);
                if (user != null)
                {
                    var company = await _context.companies
                        .Include(c => c.Subscription)
                        .FirstOrDefaultAsync(c => c.RepresentativeId == user.Id);
                    if (company != null && company.Subscription != null && company.Subscription.StripeSubscriptionId == subscription.Id)
                    {
                        company.Subscription.SubscriptionStatus = "canceled";
                        company.Subscription.SubscriptionEndDate = DateTime.UtcNow;
                        company.Subscription.IsActive = false;
                        await _context.SaveChangesAsync();
                        if (await _userManager.IsInRoleAsync(user, Constant.CompanyAdmin))
                        {
                            await _userManager.RemoveFromRoleAsync(user, Constant.CompanyAdmin);
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Company not found for user or subscription mismatch.");
                    }
                }
                else
                {
                    _logger.LogError("No user found");
                    throw new Exception("No user found");
                }
            }
        }

        private async Task HandleCustomerSubscriptionUpdatedEvent(Event stripeEvent)
        {
            var subscription = stripeEvent.Data.Object as Stripe.Subscription;
            var customerService = new CustomerService();
            var customer = await customerService.GetAsync(subscription.CustomerId);
            var subscriptionItem = subscription.Items.Data.FirstOrDefault();
            if (subscriptionItem == null)
            {
                _logger.LogError("No subscription item found in the Stripe subscription.");
                throw new Exception("No subscription item found in the Stripe subscription.");
            }

            var plan = subscriptionItem.Plan;

            if (customer.Email != null)
            {
                var user = await _userManager.FindByEmailAsync(customer.Email);
                if (user != null)
                {
                    var company = await _context.companies
                        .Include(c=>c.Subscription)
                        .ThenInclude(s=>s.Plan)
                        .FirstOrDefaultAsync(c => c.RepresentativeId == user.Id);
                    var currentPlanId = "";
                    if (company?.Subscription?.Plan != null)
                    {
                        currentPlanId = company.Subscription?.PlanId;

                    }
                    if (company != null && company.Subscription != null && company.Subscription.Plan!=null && company.Subscription.StripeSubscriptionId == subscription.Id)
                    {
                        if (!CanDowngrade(company, plan.Id))
                        {
                            _logger.LogWarning("Cannot downgrade due to exceeded usage limits.");
                            await RevertDowngrade(subscription, currentPlanId);
                            await notificationService.NotifyUserAsync(user.Id, "Downgrade failed, You have exceeded the usage limits for the selected plan.","/");
                            return;
                        }
                        var invoiceService = new InvoiceService();
                        var latestInvoice = await invoiceService.ListAsync(new InvoiceListOptions
                        {
                            Subscription = subscription.Id,
                            Limit = 1
                        });

                        var amountPaid = latestInvoice.Data.FirstOrDefault()?.AmountPaid / 100m ?? 0m;

                        company.Subscription.SubscriptionStatus = subscription.Status;
                        company.Subscription.SubscriptionEndDate = subscription.CurrentPeriodEnd;
                        company.Subscription.IsActive = subscription.Status == "active";
                        if (company.Subscription.PlanId != plan.Id)
                        {
                            company.Subscription.PlanId = plan.Id;
                        }
                        company.SubscriptionAmtPaid += amountPaid; // Update amount paid

                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _logger.LogWarning("Company not found for user or subscription mismatch.");
                    }
                }
                else
                {
                    _logger.LogError("No user found");
                    throw new Exception("No user found");
                }
            }
        }
        private async Task RevertDowngrade(Stripe.Subscription subscription, string originalPlanId)
        {
            var subscriptionService = new SubscriptionService();
            var updateOptions = new SubscriptionUpdateOptions
            {
                
                Items = new List<SubscriptionItemOptions>
        {
            new SubscriptionItemOptions
            {
                Id = subscription.Items.Data.First().Id,
                Plan = originalPlanId, // Revert to original plan
            }
        }
            };

            await subscriptionService.UpdateAsync(subscription.Id, updateOptions);
        }

        private bool CanDowngrade(Company company, string newPlanId)
        {
            int newPlanLimit = GetPlanLimit(newPlanId);
            return company.UsedPropertyCounts <= newPlanLimit;
        }

        private int GetPlanLimit(string planId)
        {
            if (planId == "price_1PaegrGFthNCZxNO1kdaJ4ct")
            {
                return 5;
            }else if (planId == "price_1PZ4m0GFthNCZxNOoti2pHeh")
            {
                return 20;
            }
            else if (planId == "price_1PZ4mUGFthNCZxNOwjVLIVd6")
            {
                return 50;
            }
            else
            {
                throw new ArgumentException("Invalid plan ID");
            }
        }

    }
}
