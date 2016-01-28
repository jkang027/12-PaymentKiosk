using PaymentKiosk.Core.Domain;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentKiosk.Core.Services
{
    public class TransactionService
    {
        private const string StripeApiKey = "sk_test_RtIitsFM3LqSFnvLh3CZbGqd";

        public static bool Charge(Customer customer, CreditCard creditCard, decimal cost)
        {
            // Call stripe API
            var chargeDetails = new StripeChargeCreateOptions();

            chargeDetails.Amount = (int)(cost * 100);
            chargeDetails.Currency = "usd";

            chargeDetails.Source = new StripeSourceOptions
            {
                Object = "card",
                Name = customer.LastName + ", " + customer.FirstName,
                Number = creditCard.CardNumber,
                ExpirationMonth = creditCard.ExpirationDate.Month.ToString(),
                ExpirationYear = creditCard.ExpirationDate.Year.ToString(),
                Cvc = creditCard.Cvc
            };

            var chargeService = new StripeChargeService(StripeApiKey);
            var response = chargeService.Create(chargeDetails);

            if (!response.Paid)
            {
                throw new Exception(response.FailureMessage);
            }

            return response.Paid;
        }
    }
}
