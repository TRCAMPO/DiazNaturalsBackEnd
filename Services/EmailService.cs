using System;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc;

public class EmailService
{   
        

        public async Task SendEmail(string emailAddress, string subject, string messageSend)
        {
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("diaznaturalsdistri@gmail.com", "zpuksazfwpqxqykq");

                var message = new MailMessage("diaznaturalsdistri@gmail.com", emailAddress, subject, messageSend);

                try
                {
                    await client.SendMailAsync(message);
                    //Console.WriteLine("Correo electrónico enviado correctamente.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al enviar el correo electrónico: " + ex.Message);

                }
            }
        }
    
}
