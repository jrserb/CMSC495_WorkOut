using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
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

            var mailbody = $@"Hello, 
This is a new contact request from Workout Buddy:

Name: {Contact.FirstName}
LastName: {Contact.LastName}
Email: {Contact.Email}
Message: ""{Contact.Message}""


Cheers,
Workout Buddy";

            SendMail(mailbody);


            return RedirectToPage("Index");
        }

        private async void SendMail(string mailbody)
        {
            using (var message = new System.Net.Mail.MailMessage(Contact.Email, "workoutbuddycmsc495@gmail.com"))
            {
                message.To.Add(new MailAddress("workoutbuddycmsc495@gmail.com"));
                message.From = new MailAddress(Contact.Email);
                message.Subject = "New Contact Request from Workout Buddy";
                message.Body = mailbody;

                using (var smtpClient = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "workoutbuddycmsc495@gmail.com",
                        Password = "cmsc495muscles"
                    };
                    smtpClient.Credentials = credential;
                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    message.From = new MailAddress("example@gmail.com");
                    await smtpClient.SendMailAsync(message);
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