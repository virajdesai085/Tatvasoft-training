using Helperland.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
            contactUs.CreatedOn = DateTime.Now;
            contactUs.CreatedBy = null;
            db.ContactUs.Add(contactu);
            db.SaveChanges();
            return View("Contact");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class ContactUs
    {
    }
}