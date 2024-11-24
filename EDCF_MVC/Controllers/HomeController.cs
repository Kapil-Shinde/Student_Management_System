using System.Diagnostics;
using EDCF_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Mail;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.EntityFrameworkCore;

namespace EDCF_MVC.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        private readonly StundentDbContext stundentDbContext;

            public HomeController(StundentDbContext stundentDbContext)
            {
               this.stundentDbContext = stundentDbContext;
            }

        public IActionResult Index()
        {
            
            return View();
        }

        [HttpGet]
        public IActionResult Authenticate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Authenticate(string username, string password, string role)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Username and Password are required.";
                return View();
            }

            // Dummy login logic based on role
            if (role == "User" && username == "user" && password == "password")
            {
                // Logic for user login
                HttpContext.Session.SetString("Role", "User");
                HttpContext.Session.SetString("Username", username);
                return RedirectToAction("Index", "Home"); // Redirect to the Index page after successful login
            }
            else if (role == "Admin" && username == "admin" && password == "password")
            {
                // Logic for admin login
                HttpContext.Session.SetString("Role", "Admin");
                HttpContext.Session.SetString("Username", username);
                return RedirectToAction("Index", "Home"); // Redirect to the Index page for admin
            }

            // If login fails
            ViewBag.ErrorMessage = "Invalid username, password, or role.";
            return View();
        }

        public IActionResult Data()
        {
            var stdData = stundentDbContext.Students.ToList();
            return View(stdData);
        }

        public IActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult Create(Student student)
            {
                

                if (ModelState.IsValid)
                {
                    // Save the student data to the database
                    stundentDbContext.Students.Add(student);
                    stundentDbContext.SaveChanges();

                    // Redirect to Index after saving
                    return RedirectToAction("Data");
                }

                return View(student);
        }

        public IActionResult Edit(int id)
        {
            //ViewBag.Data = HttpContext.Session.GetString("MyKey");
            var student = stundentDbContext.Students.FirstOrDefault(s => s.ID == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student); // Pass the student data to the Edit view
        }

        [HttpPost]
        public IActionResult Edit(Student student)
        {
            
            if (ModelState.IsValid)
            {
                // Find the existing student in the database
                var existingStudent = stundentDbContext.Students.FirstOrDefault(s => s.ID == student.ID);
                if (existingStudent == null)
                {
                    return NotFound();
                }

                // Update the student properties
                existingStudent.Name = student.Name;
                existingStudent.Age = student.Age;

                // Save changes to the database
                stundentDbContext.SaveChanges();

                // Redirect to the Index page after saving
                return RedirectToAction("Index");
            }

            // If the model state is invalid, return the same view to show validation errors
            return View(student);
        }


        // [HttpDelete]
        public IActionResult Delete(int id)
        {
          
            var student = stundentDbContext.Students.FirstOrDefault(s => s.ID == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student); // Pass the student to the Delete confirmation view
        }

        [HttpPost]
        [ActionName("Delete")] // This matches the action name in the form
        public IActionResult DeleteConfirm(int id)
        {
            
            var existingStudent = stundentDbContext.Students.FirstOrDefault(s => s.ID == id);
            if (existingStudent != null)
            {
                stundentDbContext.Students.Remove(existingStudent);
                stundentDbContext.SaveChanges();

                //Below code is for reset the ID
                var tableName = "Students"; 
                var sql = $"DBCC CHECKIDENT ('{tableName}', RESEED, {stundentDbContext.Students.Count()})";
                stundentDbContext.Database.ExecuteSqlRaw(sql);

            }
            return RedirectToAction("Data"); // Redirect to the Index view after deletion
        }

        public IActionResult Contact()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult Contact(string recipientEmail, string name, string senderEmail, string subject, string message)
        {
            //ViewBag.Data = HttpContext.Session.GetString("MyKey");

            //if (string.IsNullOrWhiteSpace(recipientEmail) ||
            //    string.IsNullOrWhiteSpace(name) ||
            //    string.IsNullOrWhiteSpace(email) ||
            //    string.IsNullOrWhiteSpace(message))
            //{
            //    ViewBag.Error = "All fields are required!";
            //    return View();
            //}

            //return View();

            try
            {
                // Configure SMTP settings
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587, // or 465 if using SSL
                    Credentials = new NetworkCredential("kapilshinde929@gmail.com", "hcglfioibjobmegf"),
                    EnableSsl = true,
                };

                // Construct the email
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = subject,
                    Body = $"Name: {name}\nEmail: {senderEmail}\nMessage: {message}",
                    IsBodyHtml = false,
                };

                mailMessage.To.Add("kapilshinde929@gmail.com"); // Use the runtime recipient email

                // Send the email
                smtpClient.Send(mailMessage);

                ViewBag.Success = "Thank you for contacting us! Your message has been sent.";
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"There was an error sending the email: {ex.Message}";
            }

            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Course()
        {
            return View();
        }

        public IActionResult BCA()
        {
            return View();
        }

        public IActionResult MCA()
        {
            return View();
        }

        public IActionResult MBA()
        {
            return View();
        }

       

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
