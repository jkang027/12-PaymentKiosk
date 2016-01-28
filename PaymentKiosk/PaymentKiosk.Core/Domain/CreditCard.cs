using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentKiosk.Core.Domain
{
    public class CreditCard
    {
        public string CardNumber { get; set; }
        public string Cvc { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Address BillingAddress { get; set; }
        public Customer Customer { get; set; }
    }
}
