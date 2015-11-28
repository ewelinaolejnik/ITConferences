using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ITConferences.WebUI.Service
{
    public class EmailClient : SmtpClient
    {
        public string UserName { get; set; }

        public EmailClient() :
        base(ConfigurationManager.AppSettings["Host"], Int32.Parse(ConfigurationManager.AppSettings["Port"]))
        {
            this.UserName = ConfigurationManager.AppSettings["UserName"];
            this.EnableSsl = Boolean.Parse(ConfigurationManager.AppSettings["Ssl"]);
            this.UseDefaultCredentials = false;
            this.Credentials = new System.Net.NetworkCredential(this.UserName, ConfigurationManager.AppSettings["Password"]);
        }
    }
}
