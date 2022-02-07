using Helperland.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace Helperland.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        projectContext db = new projectContext();

        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Prices()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactU contactu)
        {
            ContactU contactUs = new ContactU();
            contactUs.Name = contactu.Name + contactu.Name;
            contactUs.Email = contactu.Email;
            contactUs.PhoneNumber = contactu.PhoneNumber;
            contactUs.Message = contactu.Message;
            contactUs.Subject = contactu.Subject;
            contactUs.UploadFileName = contactu.FileName;
            contactUs.CreatedBy = null;
            contactUs.CreatedOn = DateTime.Now;
            db.ContactUs.Add(contactu);
            db.SaveChanges();
            return View("Contact");
        }

        public IActionResult CreateAccount()
        {
             return View();
        }

        [HttpPost]
        public IActionResult CreateAccount(User user)
        {
            User u = new User();
            u.FirstName = user.FirstName;
            u.LastName = user.LastName;
            u.Mobile = user.Mobile;
            u.Email = user.Email;
            u.Password = user.Password;
            db.Users.Add(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult ServiceProvider()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ServiceProvider(User us)
        {
            User user = new User();
            user.FirstName = us.FirstName;      
            user.LastName = us.LastName;
            user.Email = us.Email;
            user.Password = us.Password;
            user.Mobile = us.Mobile;
            db.Add(us);
            db.SaveChanges();
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            var login_user = db.Users.FirstOrDefault(x => !(!x.Email.Equals(user.Email) || !x.Password.Equals(user.Password)));
            if (login_user != null)
            {
                return RedirectToAction("FAQ");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public IActionResult ServiceHistory()
        {
            return View();
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgetPassword(User user)
        {
            var get_user = db.Users.FirstOrDefault(p => p.Email.Equals(user.Email));
            var forget = db.Users.FirstOrDefault(x => x.Email.Equals(user.Email));
            string resetCode = Guid.NewGuid().ToString();
            var verifyUrl = "/Account/ResetPassword/" + resetCode;
            
            string baseUrl = string.Format("{0}://{1}",
                       HttpContext.Request.Scheme, HttpContext.Request.Host);
            var activationUrl = $"{baseUrl}/home/ForgetPassword?code={Guid.NewGuid()}";

            if (forget != null)
            {
                var senderemail = new MailAddress("helperlandservices@gmail.com", "demo test");
                var receiveremail = new MailAddress(user.Email);

                var password = "2022#helperland";
                var sub = "Reset Password";
                var body = activationUrl;

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderemail.Address, password)
                };
                using (var message = new MailMessage(senderemail, receiveremail)
                {
                    Subject = sub,
                    Body = body
                    
                })
                {
                    smtp.Send(message);
                }
                return RedirectToAction("Index", "Home");

            }
            else
            {
                return View();

            }
        }
        public IActionResult ResetPassword(User user)
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