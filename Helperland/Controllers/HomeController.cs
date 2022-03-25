using Helperland.Models;
using Helperland_projectContext.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Helpers;

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
            if(ModelState.IsValid)
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
            return View();
        }
      
        [HttpPost]
        public IActionResult Login(User user)
        {
            User login_user = db.Users.FirstOrDefault(x => (x.Email.Equals(user.Email) && x.Password.Equals(user.Password)));
            if (login_user != null)
            {
                if (login_user.UserTypeId == 1)
                {
                    HttpContext.Session.SetInt32("User_id", login_user.UserId);
                    HttpContext.Session.SetString("UserName_Session", login_user.FirstName);
                    HttpContext.Session.SetInt32("typeid", login_user.UserTypeId);
                    TempData["username"] = login_user.FirstName + " " + login_user.LastName;
                    return RedirectToAction("CustomerDashbord", "Home");
                }
                else if (login_user.UserTypeId == 2)
                {
                    if(login_user.IsActive == true)
                    {
                        HttpContext.Session.SetInt32("User_id", login_user.UserId);
                        HttpContext.Session.SetString("UserName_Session", login_user.FirstName);
                        HttpContext.Session.SetInt32("typeid", login_user.UserTypeId);
                        TempData["username"] = login_user.FirstName + " " + login_user.LastName;
                        return RedirectToAction("ServiceProviderDashbord", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else if (login_user.UserTypeId == 0)
                {
                    HttpContext.Session.SetString("UserName_Session", login_user.FirstName);
                    return RedirectToAction("Admin");
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
            if(ModelState.IsValid)
            {
                User u = new User();
                u.FirstName = user.FirstName;
                u.LastName = user.LastName;
                u.Mobile = user.Mobile;
                u.Email = user.Email;
                u.Password = user.Password;
                u.CreatedDate = DateTime.Now;
                u.UserTypeId = 1;
                db.Users.Add(u);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult ServiceProvider()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ServiceProvider(User user)
        {
            if(ModelState.IsValid)
            {
                User us = new User();
                us.UserTypeId = 2;
                db.Users.Add(user);
                db.SaveChanges();
                return View();
            }
            return View();
        }
        [HttpPost]
        public IActionResult ForgetPassword(User user)
        {
            var u = db.Users.Where( x => x.Email == user.Email ).FirstOrDefault();


            if (u != null)
            {
                string BaseUrl = string.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);
                var ResetUrl = $"{BaseUrl}/Home/ResetPassword/" + u.UserId;
                var senderemail = new MailAddress("vrdesai2301@gmail.com", "Reset your password");
                var receiveremail = new MailAddress(user.Email);

                var password = "8849410565v";
                var sub = "Reset Password";
                var body = "Hello" + u.FirstName + ",\n\n\n You can change your password by this link.\n\n\n"+ ResetUrl + "\n\n\nThank you for visiting Helperland." ;

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
        public IActionResult ResetPassword(int? id)
        {
            User user = db.Users.Where( x => x.UserId == id ).FirstOrDefault();
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(User user)
        {
                User u = db.Users.Where(x => x.UserId == user.UserId).FirstOrDefault();
                u.Password = user.Password;
                u.ModifiedBy = user.UserId;
                u.ModifiedDate = DateTime.Now;
                db.Users.Update(u);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
        }
        public IActionResult BookService()
        {
            if (HttpContext.Session.GetInt32("User_id") != null)
            {
                return View();
            }
            return RedirectToAction("Index");
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
        public IActionResult ServiceAddress()
        {
            var userid = HttpContext.Session.GetInt32("User_id");
            List<UserAddress> all = db.UserAddresses.Where(x => x.UserId == userid).ToList();
            return View(all);
        }
        public IActionResult SaveAddress([FromBody] UserAddress userAddress)
        {
            userAddress.UserId = (int)HttpContext.Session.GetInt32("User_id");
            userAddress.IsDefault = false;
            userAddress.IsDeleted = false;
            db.UserAddresses.Add(userAddress);
            db.SaveChanges();
            return Json(true);
        }
        [HttpPost]
        public string ConfirmServiceRequest([FromBody] ServiceRequest sr)
        {
            sr.UserId = (int)HttpContext.Session.GetInt32("User_id");
            sr.ServiceHourlyRate = 9;
            sr.PaymentDue = true;
            sr.CreatedDate = DateTime.Now;
            sr.ModifiedDate = DateTime.Now;
            sr.ModifiedBy = 13;
            sr.Distance = 15;
            sr.HasPets = true;
            sr.Status = 0;
            db.ServiceRequests.Add(sr);
            db.SaveChanges();

            UserAddress userAddress = db.UserAddresses.Where(x => x.AddressId == sr.AddressId).SingleOrDefault();
            ServiceRequestAddress sra = new ServiceRequestAddress();
            sra.ServiceRequestId = sr.ServiceRequestId;
            sra.AddressLine1 = userAddress.AddressLine1;
            sra.AddressLine2 = userAddress.AddressLine2;
            sra.Mobile = userAddress.Mobile;
            sra.City = userAddress.City;
            sra.State = userAddress.State;
            sra.PostalCode = userAddress.PostalCode;
            sra.Email = userAddress.Email;
            db.ServiceRequestAddresses.Add(sra);
            db.SaveChanges();

            sr.ServiceId = 1000 + sr.ServiceRequestId;
            db.ServiceRequests.Update(sr);
            db.SaveChanges();

            return "true";
        }
        public IActionResult CustomerDashbord()
        {
            int userid = (int)HttpContext.Session.GetInt32("User_id");
            List<ServiceRequest> sr = db.ServiceRequests.Where(x => x.UserId == userid).ToList();
            return View(sr);
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
        public IActionResult CustomerEditaddress(int id , bool isAjax = false)
        {
            UserAddress address = db.UserAddresses.Where(x => x.AddressId == id).FirstOrDefault();
            if (isAjax)
            {
                //return Json(address);
                return View(address);
            }
            return View(address);

        }
        [HttpPost]
        public string CustomerEditAddress([FromBody] UserAddress Obj)
        {
            var id = Obj.AddressId;
            UserAddress useraddress = db.UserAddresses.Where(x => x.AddressId == id).FirstOrDefault();
            useraddress.AddressLine1 = Obj.AddressLine1;
            useraddress.AddressLine2 = Obj.AddressLine2;
            useraddress.Mobile = Obj.Mobile;
            useraddress.City = Obj.City;
            useraddress.PostalCode = Obj.PostalCode;
            db.UserAddresses.Update(useraddress);
            db.SaveChanges(); 
            return "true";
        }
        public IActionResult CustomerAddAddress()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CustomerAddaddress([FromBody] UserAddress obj)
        {
            obj.UserId= (int)HttpContext.Session.GetInt32("User_id");
            obj.IsDefault = false;
            obj.IsDeleted = false;
            db.UserAddresses.Add(obj);
            db.SaveChanges();
            return Json(new { issuccess = true });
        }
        public IActionResult CustomerServiceHistory()
        {
            var userid = HttpContext.Session.GetInt32("User_id");
            //List<ServiceRequest> sr = db.ServiceRequests.Where(x => x.UserId == userid).ToList();
            if(userid != null)
            {
                var sh = (from ServiceRequest in db.ServiceRequests
                          join User in db.Users
                          on ServiceRequest.ServiceProviderId equals User.UserId into A
                          from User in A.DefaultIfEmpty()
                          join Rating in db.Ratings on ServiceRequest.ServiceRequestId equals Rating.ServiceRequestId into B
                          from Rating in B.DefaultIfEmpty()
                          where ServiceRequest.UserId == userid && ServiceRequest.Status != 0
                          select new Custom
                          {
                              FirstName = User == null ? " " : User.FirstName,
                              LastName = User == null ? " " :User.LastName,
                              Ratings = Rating == null ? 0 : Rating.Ratings,
                              ServiceStartDate = ServiceRequest.ServiceStartDate,
                              TotalCost = ServiceRequest.TotalCost,
                              Status = ServiceRequest.Status,
                              ServiceRequestId = ServiceRequest.ServiceRequestId,
                              ServiceProviderId = ServiceRequest.ServiceProviderId,
                          }).ToList();
                return View(sh);
            }
            return View();
        }
        public IActionResult Reschedule(int id , bool isAjax = false)
        {
            ServiceRequest request = db.ServiceRequests.Where(x => x.ServiceRequestId == id).FirstOrDefault(); 
            if(isAjax)
            {
                return View(request);
            }
            return View(request);
        }
        public string DeleteReq([FromBody] ServiceRequest obj)
        {
            ServiceRequest req = db.ServiceRequests.Where( x => x.ServiceRequestId == obj.id).FirstOrDefault();
            req.Status = 2;
            req.Comments = obj.Comments;
            db.ServiceRequests.Update(req);
            db.SaveChanges();
            return "true";
        }
        [HttpPost]
        public string Updatetime([FromBody] ServiceRequest obj)
        {
            var id = obj.id;
            ServiceRequest sr = db.ServiceRequests.Where( x => x.ServiceRequestId == id).FirstOrDefault();
            sr.ServiceStartDate = obj.ServiceStartDate;
            db.ServiceRequests.Update(sr);
            db.SaveChanges();
            return "true";
        }
        public IActionResult CDashbordPopup(int id)
        {
            var add = (from ServiceRequest in db.ServiceRequests join ServiceRequestAddress in db.ServiceRequestAddresses 
                       on ServiceRequest.ServiceRequestId equals ServiceRequestAddress.ServiceRequestId
                       where ServiceRequest.ServiceRequestId == id 
                       select new ServiceDPopup
                       {
                           ServiceRequestId = ServiceRequest.ServiceRequestId,
                           ServiceStartDate = ServiceRequest.ServiceStartDate,
                           TotalCost = ServiceRequest.TotalCost,
                           AddressLine1 = ServiceRequestAddress.AddressLine1,
                           AddressLine2 = ServiceRequestAddress.AddressLine2,
                           Mobile = ServiceRequestAddress.Mobile,
                           City = ServiceRequestAddress.City,
                           Email = ServiceRequestAddress.Email,
                           ServiceHours = ServiceRequest.ServiceHours,
                           HasPets = ServiceRequest.HasPets,
                       }).Single();
            return View(add);
        }

        public IActionResult ServiceProviderDashbord()
        {
            return View();
        }
        public IActionResult ServiceProviderSettings()
        {
            int userid = (int)HttpContext.Session.GetInt32("User_id");
            ServiceProSettings add = new ServiceProSettings();
            add.Users = db.Users.Where(x => x.UserId == userid).FirstOrDefault();
            add.UserAddresses = db.UserAddresses.Where(x => x.UserId == userid).FirstOrDefault();
            return View(add);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ServiceProviderSettings(ServiceProSettings sp)
        {
            int userid = (int)HttpContext.Session.GetInt32("User_id");
            User u = db.Users.Where( x => x.UserId == userid).FirstOrDefault();
            u.LastName = sp.Users.LastName;
            u.FirstName = sp.Users.FirstName;
            u.Mobile = sp.Users.Mobile;
            u.Gender = sp.Users.Gender;
            db.Users.Update(u);
            db.SaveChanges();

            UserAddress ua = db.UserAddresses.Where(x => x.UserId == userid).FirstOrDefault();
            if (ua != null)
            {
                ua.AddressLine1 = sp.UserAddresses.AddressLine1;
                ua.AddressLine2 = sp.UserAddresses.AddressLine2;
                ua.PostalCode = sp.UserAddresses.PostalCode;
                ua.City = sp.UserAddresses.City;
                ua.Mobile = sp.Users.Mobile;
                db.UserAddresses.Update(ua);
                db.SaveChanges();
            }
            else
            {
                UserAddress address = new UserAddress();
                address.UserId = userid;
                address.Mobile = sp.UserAddresses.Mobile;
                address.City = sp.UserAddresses.City;
                address.AddressLine1 = sp.UserAddresses.AddressLine1;
                address.AddressLine2 = sp.UserAddresses.AddressLine2;
                address.PostalCode = sp.UserAddresses.PostalCode;
                db.UserAddresses.Add(address);
                db.SaveChanges();
            }
            HttpContext.Session.SetString("UserName_Session", u.FirstName);
            return View();
        }

        [HttpPost]
        public string changepassword([FromBody] User obj)
        {
            int userid = (int)HttpContext.Session.GetInt32("User_id");
            User u = db.Users.Where(x => x.UserId == userid).FirstOrDefault();
            if( u.Password == obj.Password )
            {
                u.Password = obj.ConfirmPassword;
                db.Users.Update(u);
                db.SaveChanges();
                return "true";
            }
            return "false";
        }
        public IActionResult SPNewServiceRequests()
        {
            int userid = (int)HttpContext.Session.GetInt32("User_id");
            User u = db.Users.Where( x => x.UserId == userid).FirstOrDefault();
            var query = (from ServiceRequest in db.ServiceRequests join ServiceRequestAddress in db.ServiceRequestAddresses
                         on ServiceRequest.ServiceRequestId equals ServiceRequestAddress.ServiceRequestId 
                         join User in db.Users on ServiceRequest.UserId equals User.UserId
                         //join FavoriteAndBlocked in db.FavoriteAndBlockeds on User.UserId equals FavoriteAndBlocked.UserId
                         where ServiceRequest.Status == 0 && ServiceRequest.ServiceProviderId == null /*&& FavoriteAndBlocked.IsBlocked != true*/
                         select new SPnewServiceRequests
                         {
                            ServiceRequestId = ServiceRequest.ServiceRequestId,
                            ServiceId = ServiceRequest.ServiceId,
                            ServiceStartDate = ServiceRequest.ServiceStartDate,
                            AddressLine1 = ServiceRequestAddress.AddressLine1,
                            AddressLine2 = ServiceRequestAddress.AddressLine2,
                            City = ServiceRequestAddress.City,
                            Mobile = ServiceRequestAddress.Mobile,
                            PostalCode = ServiceRequestAddress.PostalCode,
                            FirstName = User.FirstName,
                            LastName = User.LastName,
                            TotalCost = ServiceRequest.TotalCost
                         }).ToList();
            return View(query);
        }
        public string AcceptService(int id)
        {
            ServiceRequest sr = db.ServiceRequests.Where( x => x.ServiceRequestId == id).FirstOrDefault();
            sr.Status = 4;
            sr.SpacceptedDate = DateTime.Now;
            sr.ServiceProviderId = (int)HttpContext.Session.GetInt32("User_id");
            db.ServiceRequests.Update(sr);
            db.SaveChanges();

            var x = db.FavoriteAndBlockeds.Where(x => x.UserId == sr.ServiceProviderId && x.TargetUserId == sr.UserId).SingleOrDefault();
            if (x == null)
            {
                FavoriteAndBlocked fb = new FavoriteAndBlocked();
                fb.UserId = (int)sr.ServiceProviderId;
                fb.IsBlocked = false;
                fb.IsFavorite = false;
                fb.TargetUserId = sr.UserId;
                db.FavoriteAndBlockeds.Add(fb);
                db.SaveChanges();
            }
            return "true";
        }
        public IActionResult SPupcomingServices()
        {
            int userid = (int)HttpContext.Session.GetInt32("User_id");
            User u = db.Users.Where(x => x.UserId == userid).FirstOrDefault();
            var query = (from ServiceRequest in db.ServiceRequests
                         join ServiceRequestAddress in db.ServiceRequestAddresses
                         on ServiceRequest.ServiceRequestId equals ServiceRequestAddress.ServiceRequestId
                         join User in db.Users on ServiceRequest.UserId equals User.UserId
                         where ServiceRequest.ServiceProviderId == userid && ServiceRequest.Status == 4/*&& ServiceRequest.Status == 4*/
                         select new SPupcomingServices
                         {
                             ServiceRequestId = ServiceRequest.ServiceRequestId,
                             ServiceId = ServiceRequest.ServiceId,
                             ServiceStartDate = ServiceRequest.ServiceStartDate,
                             AddressLine1 = ServiceRequestAddress.AddressLine1,
                             AddressLine2 = ServiceRequestAddress.AddressLine2,
                             City = ServiceRequestAddress.City,
                             Mobile = ServiceRequestAddress.Mobile,
                             PostalCode = ServiceRequestAddress.PostalCode,
                             FirstName = User.FirstName,
                             LastName = User.LastName,
                             TotalCost = ServiceRequest.TotalCost
                         }).ToList();
            return View(query);
        }
        public IActionResult SPnewServicePopup(int id)
        {
            var add = (from ServiceRequest in db.ServiceRequests
                       join ServiceRequestAddress in db.ServiceRequestAddresses
                       on ServiceRequest.ServiceRequestId equals ServiceRequestAddress.ServiceRequestId
                       join User in db.Users on ServiceRequest.UserId equals User.UserId
                       where ServiceRequest.ServiceRequestId == id /*&& ServiceRequest.Status == 4*/
                       select new SPupcomingServices
                       {
                           ServiceRequestId = ServiceRequest.ServiceRequestId,
                           ServiceId = ServiceRequest.ServiceId,
                           ServiceStartDate = ServiceRequest.ServiceStartDate,
                           AddressLine1 = ServiceRequestAddress.AddressLine1,
                           AddressLine2 = ServiceRequestAddress.AddressLine2,
                           City = ServiceRequestAddress.City,
                           Mobile = ServiceRequestAddress.Mobile,
                           PostalCode = ServiceRequestAddress.PostalCode,
                           FirstName = User.FirstName,
                           LastName = User.LastName,
                           TotalCost = ServiceRequest.TotalCost
                       }).Single();
            return View(add);
        }
        public IActionResult SPupComingServicePopup(int id)
        {
            var add = (from ServiceRequest in db.ServiceRequests
                       join ServiceRequestAddress in db.ServiceRequestAddresses
                       on ServiceRequest.ServiceRequestId equals ServiceRequestAddress.ServiceRequestId
                       join User in db.Users on ServiceRequest.UserId equals User.UserId
                       where ServiceRequest.ServiceRequestId == id /*&& ServiceRequest.Status == 4*/
                       select new SPupcomingServices
                       {
                           ServiceRequestId = ServiceRequest.ServiceRequestId,
                           ServiceId = ServiceRequest.ServiceId,
                           ServiceStartDate = ServiceRequest.ServiceStartDate,
                           AddressLine1 = ServiceRequestAddress.AddressLine1,
                           AddressLine2 = ServiceRequestAddress.AddressLine2,
                           City = ServiceRequestAddress.City,
                           Mobile = ServiceRequestAddress.Mobile,
                           PostalCode = ServiceRequestAddress.PostalCode,
                           FirstName = User.FirstName,
                           LastName = User.LastName,
                           TotalCost = ServiceRequest.TotalCost
                       }).Single();
            return View(add);
        }
        public string CompleteService(int id)
        {
            ServiceRequest sr = db.ServiceRequests.Where(x => x.ServiceRequestId == id).FirstOrDefault();
            sr.Status = 1;
            sr.SpacceptedDate = DateTime.Now;
            db.ServiceRequests.Update(sr);
            db.SaveChanges();
            return "true";
        }
        public string CancelService(int id)
        {
            ServiceRequest sr = db.ServiceRequests.Where(x => x.ServiceRequestId == id).FirstOrDefault();
            sr.Status = 2;
            sr.SpacceptedDate = DateTime.Now;
            db.ServiceRequests.Update(sr);
            db.SaveChanges();
            return "true";
        }
        public IActionResult SPhistory()
        {
            int userid = (int)HttpContext.Session.GetInt32("User_id");
            User u = db.Users.Where(x => x.UserId == userid).FirstOrDefault();
            var query = (from ServiceRequest in db.ServiceRequests
                         join ServiceRequestAddress in db.ServiceRequestAddresses
                         on ServiceRequest.ServiceRequestId equals ServiceRequestAddress.ServiceRequestId
                         join User in db.Users on ServiceRequest.UserId equals User.UserId
                         where ServiceRequest.ServiceProviderId == userid && ServiceRequest.Status == 1/*&& ServiceRequest.Status == 4*/
                         select new SPupcomingServices
                         {
                             ServiceRequestId = ServiceRequest.ServiceRequestId,
                             ServiceStartDate = ServiceRequest.ServiceStartDate,
                             AddressLine1 = ServiceRequestAddress.AddressLine1,
                             AddressLine2 = ServiceRequestAddress.AddressLine2,
                             City = ServiceRequestAddress.City,
                             Mobile = ServiceRequestAddress.Mobile,
                             PostalCode = ServiceRequestAddress.PostalCode,
                             FirstName = User.FirstName,
                             LastName = User.LastName,
                         }).ToList();
            return View(query);
        }
        public IActionResult SPblockCustomer()
        {
            int userid = (int)HttpContext.Session.GetInt32("User_id");
            var query = (from User in db.Users join FavoriteAndBlocked in db.FavoriteAndBlockeds 
                         on User.UserId equals FavoriteAndBlocked.TargetUserId
                         where FavoriteAndBlocked.UserId == userid    
                         select new favblock
                         {
                            Id = FavoriteAndBlocked.Id,
                            UserId = User.UserId,
                            FirstName = User.FirstName,
                            LastName = User.LastName,
                            IsBlocked = FavoriteAndBlocked.IsBlocked,
                         }).ToList();
            return View(query);
        }
        public string block(int id)
        {
            FavoriteAndBlocked fb = db.FavoriteAndBlockeds.Where( x => x.Id == id).FirstOrDefault();
            fb.IsBlocked = true;
            db.FavoriteAndBlockeds.Update(fb);
            db.SaveChanges();
            return "true";
        }
        public string unblock(int id)
        {
            FavoriteAndBlocked fb = db.FavoriteAndBlockeds.Where(x => x.Id == id).FirstOrDefault();
            fb.IsBlocked = false;
            db.FavoriteAndBlockeds.Update(fb);
            db.SaveChanges();
            return "true";
        }
        public IActionResult RateSP(int spid)
        {
            User u = db.Users.Where(x => x.UserId == spid).FirstOrDefault();
            return View(u);
        }
        public string rate([FromBody] Rating rate)
        {
            int userid = (int)HttpContext.Session.GetInt32("User_id");
            Rating r = db.Ratings.Where( x => x.ServiceRequestId == rate.ServiceRequestId ).FirstOrDefault();

            if( r != null)
            {
                r.RatingDate = DateTime.Now;
                r.Ratings = rate.Ratings;
                r.Comments = rate.Comments;
                r.Friendly = rate.Friendly;
                r.OnTimeArrival = rate.OnTimeArrival;
                r.QualityOfService = rate.QualityOfService;
                db.Ratings.Update(r);
                db.SaveChanges();
                
            }
            else
            {
                rate.RatingFrom = userid;
                rate.RatingDate = DateTime.Now;
                db.Ratings.Add(rate);
                db.SaveChanges();
            }
            return "true";
        }

        public IActionResult Admin()
        {   
            var query =(from ServiceRequest in db.ServiceRequests
                        join ServiceRequestAddress in db.ServiceRequestAddresses on
                        ServiceRequest.ServiceRequestId equals ServiceRequestAddress.ServiceRequestId
                        join user in db.Users on ServiceRequest.UserId equals user.UserId
                        join rating in db.Ratings on ServiceRequest.ServiceRequestId equals rating.ServiceRequestId into A
                        from rating in A.DefaultIfEmpty()
                        join sp in db.Users on ServiceRequest.ServiceProviderId equals sp.UserId into B
                        from sp in B.DefaultIfEmpty()
                        select new Custom
                        {
                            ServiceRequestId = ServiceRequest.ServiceRequestId,
                            ServiceStartDate = ServiceRequest.ServiceStartDate,
                            ServiceProviderId = ServiceRequest.ServiceProviderId,
                            AddressLine1 = ServiceRequestAddress.AddressLine1,
                            AddressLine2 = ServiceRequestAddress.AddressLine2,
                            City = ServiceRequestAddress.City,
                            Mobile = ServiceRequestAddress.Mobile,
                            Status = ServiceRequest.Status,
                            PostalCode = ServiceRequestAddress.PostalCode,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            spFirstName = sp == null ? " " : sp.FirstName,
                            spLastName = sp == null ? " " : sp.LastName,
                            Ratings = rating == null ? 0 : rating.Ratings,
                        }).ToList();
            return View(query);
        }
        public IActionResult UserManagement()
        {
            var query = (from User in db.Users join UserAddress in db.UserAddresses
                         on User.UserId equals UserAddress.UserId
                         select new UserManagement
                         {
                             UserId = User.UserId,
                             IsActive = User.IsActive,
                             FirstName = User.FirstName,
                             LastName= User.LastName,
                             PostalCode = UserAddress.PostalCode,
                             Mobile = User.Mobile,
                             CreatedDate = User.CreatedDate,
                             UserTypeId = User.UserTypeId,

                         }).ToList();
            return View(query);
        }
        public IActionResult EditReschedule(int id)
        {
            var er =(from sr in db.ServiceRequests join sra in db.ServiceRequestAddresses 
                     on sr.ServiceRequestId equals sra.ServiceRequestId
                     where sr.ServiceRequestId == id
                     select new Editreschedule
                     {
                         UserId = sr.UserId,
                         AddressLine1 = sra.AddressLine1,
                         AddressLine2 = sra.AddressLine2,
                         PostalCode = sra.PostalCode,
                         ServiceRequestId = sr.ServiceRequestId,
                         ServiceStartDate = sr.ServiceStartDate,
                         City = sra.City,
                     }).Single();
            
            return View(er);
        }
        [HttpPost]
        public string Editreschedule([FromBody] Editreschedule add)
        {
            ServiceRequest sr = db.ServiceRequests.Where(x => x.ServiceRequestId == add.ServiceRequestId).FirstOrDefault();
            sr.ServiceStartDate = add.ServiceStartDate;
            sr.Comments = add.Comments;
            db.ServiceRequests.Update(sr);
            db.SaveChanges();

            ServiceRequestAddress sra = db.ServiceRequestAddresses.Where(x => x.ServiceRequestId == add.ServiceRequestId).FirstOrDefault();
            sra.AddressLine1 = add.AddressLine1;
            sra.AddressLine2 = add.AddressLine2;
            sra.PostalCode = add.PostalCode;
            sra.City = add.City;
            db.ServiceRequestAddresses.Update(sra);
            db.SaveChanges();

            return "true";
        }
        public string deactivate(int id)
        {
            User user = db.Users.Where(x => x.UserId == id).FirstOrDefault();
            user.IsActive = false;
            db.Users.Update(user);
            db.SaveChanges();
            return "true";
        }
        public string activate(int id)
        {
            User user = db.Users.Where(x => x.UserId == id).FirstOrDefault();
            user.IsActive = true;
            db.Users.Update(user);
            db.SaveChanges();
            return "true";
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
