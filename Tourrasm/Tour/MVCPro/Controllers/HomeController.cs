using Microsoft.AspNetCore.Mvc;
using MVCPro.Data;
using MVCPro.Models;
using System.Diagnostics;
using MVCPro.Shared;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
namespace MVCPro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        private readonly IHostingEnvironment _host;
        private AppDbContext _db;

        public HomeController(AppDbContext _db, IHostingEnvironment host)
        {
            this._db = _db;
            _host = host;
        }
        public IActionResult Index()
        {
            List<Trip> t = _db.trips.Take(6).ToList();
            ViewData["trips"] = t;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult k()
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
