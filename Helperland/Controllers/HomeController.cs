using Helperland_projectContext.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Helperland_projectContext.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        helperland_projectContext db = new helperland_projectContext();
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
            _ = db.SaveChanges();
            return View("Contact");
        }
      
        [HttpPost]
        public IActionResult Login(User user)
        {
            User login_user = db.Users.FirstOrDefault(x => (x.Email.Equals(user.Email) && x.Password.Equals(user.Password)));
            if (login_user != null)
            {
                if(login_user.UserTypeId == 1)
                {
                    HttpContext.Session.SetInt32("User_id" , login_user.UserId);
                    HttpContext.Session.SetString("UserName_Session", login_user.FirstName);
                    TempData["username"] = login_user.FirstName + " " + login_user.LastName;
                    return RedirectToAction("FAQ", "Home");
                }
                else if(login_user.UserTypeId == 2)
                {
                   HttpContext.Session.SetInt32("User_id", login_user.UserId);
                    TempData["username"] = login_user.FirstName + " " + login_user.LastName;
                   return RedirectToAction("About", "Home");  
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.message = "Username or Password is Incorrect. Please try again";
                return RedirectToAction("Prices");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("User_id");
            return RedirectToAction("Index");
        }
        public IActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateAccount(User user)
        {
            User u = new User();
            u.CreatedDate = DateTime.Now;
            u.UserTypeId = 1;
            db.Users.Add(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult ServiceProvider()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ServiceProvider(User user)
        {
            User us = new User();
            us.UserTypeId = 2;
            db.Users.Add(user);
            db.SaveChanges();
            return View();
        }
        [HttpPost]
        public IActionResult ForgetPassword(User user)
        {
            var forget = db.Users.FirstOrDefault(x => x.Email.Equals(user.Email));
            string resetCode = Guid.NewGuid().ToString();
            var verifyUrl = "/Account/ResetPassword/" + resetCode;

            string baseUrl = string.Format("{0}://{1}",
                       HttpContext.Request.Scheme, HttpContext.Request.Host);
            var activationUrl = $"{baseUrl}/home/ResetPassword?code={Guid.NewGuid()}";

            if (forget != null)
            {
                var senderemail = new MailAddress("vrdesai2301@gmail.com", "Reset your password");
                var receiveremail = new MailAddress(user.Email);

                var password = "8849410565v";
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
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(User user)
        {
            var reset = db.Users.FirstOrDefault(x => x.Email.Equals(user.Email));
            if (reset != null)
            {
                reset.Password = user.Password;
                db.Users.Update(reset);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("FAQ", "Home");
            }
        }
        public IActionResult BookService()
        {
            List<UserAddress> add = db.UserAddresses.Where(x => x.UserId == 3).ToList();
            System.Threading.Thread.Sleep(2000);
            return View(add);
            //return View();
        }
        [HttpPost]
        public string zipcode([FromBody] string postalcode )
        {
            string a;
            Zipcode p = db.Zipcodes.Where(x => x.ZipcodeValue.Equals(postalcode)).SingleOrDefault();
            if (p != null)
            {
                a = "true";
            }
            else
            {
                a="false";
            }
            return a;
        }

        public IActionResult SaveAddress([FromBody] UserAddress userAddress)
        {
            userAddress.UserId = 3;
            userAddress.IsDefault = false;
            userAddress.IsDeleted = false;
            db.UserAddresses.Add(userAddress);
            db.SaveChanges();
            return Json(true);
        }
        public IActionResult CompleteBook([FromBody] ServiceRequest serviceRequest)
        {
            serviceRequest.UserId = 3;
            serviceRequest.Distance = 15;
            serviceRequest.ServiceHourlyRate = 10;
            serviceRequest.PaymentDue = true;
            serviceRequest.CreatedDate = DateTime.Now;
            serviceRequest.ModifiedDate = DateTime.Now;
            serviceRequest.ServiceStartDate = DateTime.Now;
            serviceRequest.ModifiedBy = 3;
            serviceRequest.CreatedDate = DateTime.Now;
            serviceRequest.ModifiedDate = DateTime.Now;
            db.ServiceRequests.Add(serviceRequest);
            db.SaveChanges();
            return Json(true);
        }
        public IActionResult CustomerDashbord()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CustomerSettings()
        {
            int userid = (int)HttpContext.Session.GetInt32("User_id");
            User user = db.Users.Where(x => x.UserId == userid).FirstOrDefault();
            return View(user);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CustomerSettings(User user)
        {
            User u = db.Users.Where(x => x.UserId == user.UserId).FirstOrDefault();
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            if(ModelState.IsValid)
            {
                u.FirstName = user.FirstName;
                u.LastName = user.LastName;
                u.Mobile = user.Mobile;
                u.DateOfBirth = user.DateOfBirth;
                db.Users.Update(u);
                db.SaveChanges();

                var username = user.FirstName + ":" + user.LastName;
                HttpContext.Session.SetString("UserName_Session", username);
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CustomerUpdatePassword(User user)
        {
            int userid = (int)HttpContext.Session.GetInt32("User_id");
            var u = db.Users.Where(x => x.UserId == userid).FirstOrDefault();
            
                if(user.Password == u.Password)
                {
                    u.Password = user.ConfirmPassword;
                    db.Users.Update(u);
                    db.SaveChanges();
                    ViewBag.alert = "Succesfull";
                    return RedirectToAction("CustomerSettings");
                }
                else
                {
                    ViewBag.alert = "Error";
                return RedirectToAction("CustomerSettings");
                }
        }
        [HttpGet]
        public IActionResult CustomerAddress()
        {
            int userid = (int)HttpContext.Session.GetInt32("User_id");
            List<UserAddress> address = db.UserAddresses.Where(x => x.UserId == userid).ToList();
            return View(address);
        }

        [HttpPost]
        public string DeleteAddress(int Id)
        {
            UserAddress userAddress = db.UserAddresses.Where(x => x.AddressId == Id).FirstOrDefault();
            db.UserAddresses.Remove(userAddress);
            db.SaveChanges();
            return "true";
        }
        public IActionResult CustomerAddressAddOrEdit(int id)
        {
            if(id == 0)
            {
                return View();
            }
            else
            {
                UserAddress address = db.UserAddresses.Where(x => x.AddressId == id).FirstOrDefault();
                return View(address);
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
