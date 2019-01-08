using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public class CloudMailService : IMailService
    {
        private string mailTo = "admin@company.com";
        private string mailFrom = "noreply@mycompany.com";
        public void Send(string subject, string message)
        {
            Debug.WriteLine($"Mail from {mailFrom} to {mailTo}, with CloudMailService.");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");
        }
    }
}
