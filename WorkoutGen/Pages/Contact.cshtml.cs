using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WorkoutGen.Pages
{
    public class ContactModel : PageModel
    {
        public void OnGet()
        {

        }

        //kent contact form code
        [BindProperty]
        public ContactFormModel Contact { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var mailbody = $@"Hello website owner, 
This is a new contact request from your website:

Name: {Contact.FirstName}
LastName: {Contact.LastName}
Email: {Contact.Email}
Message: ""{Contact.Message}""


Cheers,
The websites contact form";

            SendMail(mailbody);


            return RedirectToPage("Index");
        }

        private void SendMail(string mailbody)
        {
            using (var message = new System.Net.Mail.MailMessage(Contact.Email, "me@mydomain.com"))
            {
                message.To.Add(new MailAddress("me@mydomain.com"));
                message.From = new MailAddress(Contact.Email);
                message.Subject = "New E-Mail from my website";
                message.Body = mailbody;

                using (var smtpClient = new SmtpClient("mail.mydomain.com"))
                {
                    smtpClient.Send(message);
                }
            }
        }
    }

    //kent contact form code
    public class ContactFormModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Message { get; set; }
    }
}