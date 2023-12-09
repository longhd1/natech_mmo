using System;
using System.Net;
using System.Net.Mail;

namespace NATechProxy;

public class bllEmail
{
	public void SendEmail(string toEmail, string fromEmail, string passWord, string subject, string htmlString)
	{
		try
		{
			MailMessage mailMessage = new MailMessage();
			SmtpClient smtpClient = new SmtpClient();
			mailMessage.From = new MailAddress(fromEmail);
			mailMessage.To.Add(new MailAddress(toEmail));
			mailMessage.Subject = subject;
			mailMessage.IsBodyHtml = true;
			mailMessage.Body = htmlString;
			smtpClient.Port = 587;
			smtpClient.Host = "smtp.gmail.com";
			smtpClient.EnableSsl = true;
			smtpClient.UseDefaultCredentials = false;
			smtpClient.Credentials = new NetworkCredential(fromEmail, passWord);
			smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
			smtpClient.Send(mailMessage);
		}
		catch (Exception)
		{
		}
	}
}
