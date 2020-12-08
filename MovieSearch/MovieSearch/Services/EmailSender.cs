using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Services
{
    public class EmailSender: IEmailSender
    {
        private readonly string _host;

        private readonly int _port;

        private readonly string _user;

        private readonly string _password;

        public EmailSender(string host, int port, string user, string password)
        {
            _host = host;
            _port = port;
            _user = user;
            _password = password;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress ("Admin", _user));
            msg.To.Add(new MailboxAddress("", email));
            msg.Subject = subject;
            msg.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_host, _port, false);
                await client.AuthenticateAsync(_user, _password);
                await client.SendAsync(msg);

                await client.DisconnectAsync(true);

            }
   
        }
    }
}
