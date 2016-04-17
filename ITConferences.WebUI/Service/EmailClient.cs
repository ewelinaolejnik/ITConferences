using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace ITConferences.WebUI.Service
{
    public class EmailClient : SmtpClient
    {
        public EmailClient() :
            base(ConfigurationManager.AppSettings["Host"], int.Parse(ConfigurationManager.AppSettings["Port"]))
        {
            UserName = ConfigurationManager.AppSettings["UserName"];
            EnableSsl = bool.Parse(ConfigurationManager.AppSettings["Ssl"]);
            UseDefaultCredentials = false;
            Credentials = new NetworkCredential(UserName, ConfigurationManager.AppSettings["Password"]);
        }

        public string UserName { get; set; }
    }
}