using ConsumidorRabbitMQ.Domain;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ConsumidorRabbitMQ.Application
{
    public class SendEmail
    {
        public void Excexute(Mail param)
        {
            string emailFom = "felipebgonzalez@hotmail.com";
            string passwordFrom = "Felipe$33Felipe$33";


            //*****************   HOTMAIL   ****************************
            SmtpClient client = new SmtpClient("smtp-mail.outlook.com");
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential(emailFom, passwordFrom);
            client.EnableSsl = true;
            client.Credentials = credentials;
            client.TargetName = "smtp-mail.outlook.com";
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(emailFom, string.Empty, System.Text.Encoding.UTF8);
                mail.To.Add(new MailAddress(param.Email));
                mail.Subject = param.Subject;
                mail.Body = param.Body;
                client.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }



            //*****************   GMAIL   ****************************
            //MailMessage mail = new MailMessage();

            //mail.From = new MailAddress(emailFom);
            //mail.To.Add(param.Email);
            //mail.Subject = param.Subject;
            //mail.Body = param.Body;

            //using (var smtp = new SmtpClient("smtp.gmail.com"))
            //{
            //    smtp.EnableSsl = true;
            //    smtp.Port = 587;
            //    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //    smtp.UseDefaultCredentials = false;

            //    // seu usuário e senha para autenticação
            //    smtp.Credentials = new NetworkCredential(emailFom, passwordFrom);

            //    // envia o e-mail
            //    smtp.Send(mail);
            //}


        }
    }
}
