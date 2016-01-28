using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using System.IO;

namespace PaymentKiosk.Core.Services
{
    public class SendSms
    {
        private const string AccountSid = "ACd38f53f3eb4f38b4ce691b1057f09eea";
        private const string AuthToken = "999256053bfe041acd8d172df7d6ca35";
        private const string MyNumber = "+18185732611";

        public static void SendMessage(string to, string message)
        {
            var twilio = new TwilioRestClient(AccountSid, AuthToken);
            twilio.SendMessage(MyNumber, to, message);
        }
    }
}
