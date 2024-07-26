﻿using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;

namespace RealEstate.Infrastructure.Services.Subscription
{
    public class StripeProductsService
    {
        private readonly StripeClient _stripeClient;
        private readonly IConfiguration configuration;
        private readonly string secretKey;

        public StripeProductsService(IConfiguration configuration)
        {
            secretKey = configuration["Stripe:SecretKey"];

            _stripeClient = new StripeClient(secretKey);
            this.configuration = configuration;
        }

        public async Task<Stripe.Product> CreateProductAsync(string name, string description)
        {
            var productOptions = new ProductCreateOptions
            {
                Name = name,
                Description = description, // Set the description here
                Metadata = new Dictionary<string, string>
        {
            { "feature_in_portal", "true" } // Custom metadata to track
        }
            };

            var productService = new Stripe.ProductService(_stripeClient);
            var product = await productService.CreateAsync(productOptions);

            // Return the created product
            return product;
        }

        public async Task<Price> CreatePriceAsync(long amount, string currency, string productId)
        {
            var priceOptions = new PriceCreateOptions
            {
                UnitAmount = amount * 100,
                Currency = currency,
                Recurring = new PriceRecurringOptions { Interval = "month" },
                Product = productId,
            };
            var priceService = new PriceService(_stripeClient);
            return await priceService.CreateAsync(priceOptions);
        }

        public async Task<string> CreatePaymentLinkAsync(string priceId)
        {
            var sessionOptions = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Price = priceId,
                    Quantity = 1,
                },
            },
                Mode = "subscription",
                SuccessUrl = "https://localhost:4200/",
                CancelUrl = "https://localhost:4200/",

            };
            var sessionService = new SessionService(_stripeClient);
            var session = await sessionService.CreateAsync(sessionOptions);
            return session.Url;
        }

        public async Task DeleteProductAsync(string productId)
        {
            var productService = new Stripe.ProductService(_stripeClient);
            var priceService = new Stripe.PriceService(_stripeClient);

            // Retrieve all prices associated with the product
            var prices = await priceService.ListAsync(new PriceListOptions { Product = productId });

            // Deactivate each price
            foreach (var price in prices)
            {
                await priceService.UpdateAsync(price.Id, new PriceUpdateOptions { Active = false });
            }

            // Now deactivate the product
            await productService.UpdateAsync(productId, new ProductUpdateOptions { Active = false });

            // Attempt to delete the product
            try
            {
                await productService.DeleteAsync(productId);
            }
            catch (StripeException ex)
            {
                // Handle any exceptions that occur during deletion
                Console.WriteLine($"Failed to delete product: {ex.Message}");
            }
        }

    }
}
