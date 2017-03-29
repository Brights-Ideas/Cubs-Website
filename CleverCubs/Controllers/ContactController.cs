using CleverCubs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace CleverCubs.Controllers
{
    public class ContactController : SurfaceController
    {
        // POST: Contact
        [HttpPost]
        public ActionResult Submit(ContactModel model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            // Get current page properties
            var recipientProp = CurrentPage.GetProperty("recipientEmailAddress");
            var subjectProp = CurrentPage.GetProperty("emailSubject");
            var thankYouPageProp = CurrentPage.GetProperty("thankYouPage");
            var senderProp = CurrentPage.GetProperty("senderEmailAddress");

            if (recipientProp == null || recipientProp.Value == null || recipientProp.Value.ToString().Length == 0)
            {
                throw new MissingFieldException("The 'Recipient Email Address' property has not been completed");
            }

            if (subjectProp == null || subjectProp.Value == null || subjectProp.Value.ToString().Length == 0)
            {
                throw new MissingFieldException("The 'Email Subject' property has not been completed");
            }

            if (thankYouPageProp?.Value == null || thankYouPageProp.Value.ToString().Length == 0)
            {
                throw new MissingFieldException("The 'Thank You Page' property has not been completed");
            }

            if (senderProp == null || senderProp.Value == null || senderProp.Value.ToString().Length == 0)
            {
                throw new MissingFieldException("The 'Sender Email Address' property has not been completed");
            }
            
            //send e-mail
            SendSmtpEmail(recipientProp.Value.ToString(), senderProp.Value.ToString(), subjectProp.Value.ToString(), model);
            //TempData["success"] = true;
            //redirect to current page to clear the form
            //return RedirectToCurrentUmbracoPage();

            //Or redirect to specific page
            return RedirectToUmbracoPage(Convert.ToInt32(thankYouPageProp.Value));
        }


        private void SendSmtpEmail(string recipientEmail, string senderEmail, string subject, ContactModel model)
        {
            StringBuilder body = new StringBuilder();
            body.AppendLine("Hi,");
            body.AppendLine();
            body.AppendLine("Name: " + model.Name);
            body.AppendLine("Email: " + model.Email);
            body.AppendLine("Comment: " + model.Message);
            body.AppendLine();
            body.AppendLine();
            body.AppendLine("Regards,");
            body.AppendLine();
            body.AppendLine("Clever Cubs");


            MailMessage mailMessage = new MailMessage();
            mailMessage.To.Add(new MailAddress(recipientEmail));
            mailMessage.Sender = new MailAddress(senderEmail);
            mailMessage.From = new MailAddress(senderEmail);

            mailMessage.Body = body.ToString();
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = false;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Send(mailMessage);
        }
    }
}