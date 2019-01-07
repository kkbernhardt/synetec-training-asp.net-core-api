using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public class LocalMailService
    {
        public string MailTo { get; set; }
        public string MailFrom { get; set; }
        
        public void Send(string subject, string message)
        {
            //send mail - output to debug window
            Debug.WriteLine($"Mail from {MailFrom} to {MailTo}, with LocalMailService.");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");
        }
    }
}
