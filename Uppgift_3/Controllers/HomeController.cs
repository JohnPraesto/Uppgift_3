using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Uppgift_3.Data;
using Uppgift_3.Models;

namespace Uppgift_3.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // POST: Home/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string employeeName, string password)
        {
            var employee = await _db.Employees
                .FirstOrDefaultAsync(d => d.EmployeeName == employeeName && d.Password == password);

            if (employee != null)
            {
                HttpContext.Session.SetString("IsAdmin", employee.IsAdmin.ToString());
                HttpContext.Session.SetString("EmployeeName", employee.EmployeeName);
                return RedirectToAction("Welcome");
            }

            ViewBag.ErrorMessage = "Invalid login attempt. Please try again.";
            return View("Index");
        }

        // GET: Driver/Welcome
        public IActionResult Welcome(DateTime? startTime, DateTime? endTime)
        {
            if (HttpContext.Session.GetString("EmployeeName") == null)
            {
                return RedirectToAction("Login");
            }

            DateTime defaultStartTime = DateTime.Now.AddHours(-24);
            DateTime defaultEndTime = DateTime.Now;

            // Use provided times or default to last 24 hours
            DateTime filterStartTime = startTime ?? defaultStartTime;
            DateTime filterEndTime = endTime ?? defaultEndTime;

            var incidents = _db.Incidents
                .Where(i => i.TimeOfIncident >= filterStartTime && i.TimeOfIncident <= filterEndTime)
                .Include(i => i.Driver)
                .ToList();

            ViewBag.FilterStartTime = filterStartTime;
            ViewBag.FilterEndTime = filterEndTime;

            ViewBag.EmployeeName = HttpContext.Session.GetString("EmployeeName");
            return View(incidents);
        }

        // GET: Home/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("EmployeeName");
            return RedirectToAction("Index");
        }
    }
}
