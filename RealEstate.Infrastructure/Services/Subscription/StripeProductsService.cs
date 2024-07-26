using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using Stripe.Climate;


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

            // Update customer portal settings to include new product

            return await productService.CreateAsync(productOptions);

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
            await productService.UpdateAsync(productId, new ProductUpdateOptions { Active = false });
            await productService.DeleteAsync(productId);
        }
    }
}
