using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Services
{
    public class StripeService
    {
        public async Task<string> CreateCheckoutSession(string planId)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = planId,
                        Quantity = 1,
                    },
                },
                Mode = "subscription",
                SuccessUrl = "http://localhost:4200/success",
                CancelUrl = "http://localhost:4200/cancel",
            };
            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            return session.Id;
        }
    
    }
}
