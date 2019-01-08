using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public class LocalMailService : IMailService
    {
        private string mailTo = "admin@mycompany.com";
        private string mailFrom = "noreply@mycompany.com";
        
        public void Send(string subject, string message)
        {
            //send mail - output to debug window
            Debug.WriteLine($"Mail from {mailFrom} to {mailTo}, with LocalMailService.");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");
        }
    }
}
